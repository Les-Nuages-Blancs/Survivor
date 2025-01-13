using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


[System.Serializable]
public struct MapRange{
    public float fromMin;
    public float fromMax;
    public float toMin;
    public float toMax;
}

//a minimal list of relevant data that the server needs to know during bitflag handling. Maybe not the most optimized solution but works, easy to implement and to extend.
public struct LootMetaData : INetworkSerializeByMemcpy{
    public LootCategory lootCategory;
    public int killedLevel;
    //other metadata ...
    //beware : if for some reason you need to add supports for complex type then must implement InetworkSerializable but int / float / enum are fine implementing ByMemcpy
}

public class LootManager : NetworkBehaviour
{
    // Singleton
    private static LootManager _instance;
    public static LootManager Instance => _instance ?? (_instance = new LootManager());
    private LootManager() { }


    // Dictionary to hold loot prefabs keyed by an identifier
    [SerializeField] private List<GameObject> lootPrefabs;
    
    //while we could arguably use the same list of network object as it would spare us other dependencies, this is bad practice and it would also be troublesome when implementing loot event. That's why I choosed to use another list even if it hads dependencies
    //[SerializeField] private NetworkPrefabsList lootPrefabs;
    
    //we can't pass GameObject as RPC arguments I think this mess is pretty much requiered
    private Dictionary<int, GameObject> hashmapID_GO; 
    private Dictionary<GameObject, int> hashmapGO_ID;

    //xp scaling
    [Tooltip("Profile of the XP given per level of mob killed. Keep values from 0 to one")]
    [SerializeField] private AnimationCurve xpScaling;
    [Tooltip("Level range of the mob / xp range")]
    [SerializeField] private MapRange xpScalingMapRange;


    private void InitializeDictionary()
    {
        hashmapID_GO = new Dictionary<int, GameObject>();
        hashmapGO_ID = new Dictionary<GameObject, int>();
        for (int i = 0; i < lootPrefabs.Count; i++){
            hashmapID_GO.Add(i, lootPrefabs[i]);
            hashmapGO_ID.Add(lootPrefabs[i], i);
        }
    }

    GameObject hm_getGO(int id){
        if (hashmapID_GO.TryGetValue(id, out GameObject prefab)){
            return prefab;
        }
        else{
            Debug.LogError($"Didn't found matching GO for given ID");
            return null;
        }
    }
    int hm_getID(GameObject go){
        if (hashmapGO_ID.TryGetValue(go, out int id)){
            return id;
        }
        else{
            Debug.LogError($"Didn't found matching id for given GO");
            return 0;
        }
    }

    float getXpByLevel(int level){
        float v = (level - xpScalingMapRange.fromMin) / (xpScalingMapRange.fromMax - xpScalingMapRange.fromMin);
        return (xpScaling.Evaluate(v)+xpScalingMapRange.toMin) * (xpScalingMapRange.fromMax - xpScalingMapRange.fromMin);
    }

    private void Awake(){
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool DropAttempt(LootEntry entry/*, Player killer*/){
        float mult_fac = 1f;
        float add_fac = 0f;
        //Bonus de drop
        if ((entry.category & LootCategory.XP) != 0){
            mult_fac += 0f; /* killer.XP_boost_mult + global.XP_boost_mult*/
            add_fac += 0f; /* killer.XP_boost_add + global.XP_boost_add*/
        }
        if ((entry.category & LootCategory.RareDrop) != 0){
            mult_fac += 0f; /* killer.RareDrop_boost_mult + global.RareDrop_boost_mult*/
            add_fac += 0f; /* killer.RareDrop_boost_mult + global.RareDrop_boost_mult*/
        }
        float roll = Random.value;
        float threshold = mult_fac * entry.dropChance + add_fac;
        //Debug.Log("roll: "+roll +" seuil: "+ (mult_fac * entry.dropChance + add_fac));
        return roll < threshold ;
    }
    
    //Il faut une référence vers le joueur qui prend le kill si on veut des bonus de drop individuel. C'est du mauvais game design de faire ça mais on se ferme pas la porte
    public void ProcessLootTable(LootTable lootTable, GameObject killed/*, Player killer*/)    {
        Vector3 position = killed.transform.position;
        foreach (LootEntry entry in lootTable.lootEntries)
        {
            if (DropAttempt(entry/*, killer*/)){
                LootMetaData lmd = new LootMetaData();
                lmd.lootCategory = entry.category;
                lmd.killedLevel = killed.GetComponent<StatistiquesLevelSystem>().CurrentLevel;


                int prefabId = hm_getID(entry.prefab);
                RequestLootInstantiation(prefabId, position, lmd);
            }
        }
    }

    // instantiate GO if server, ask server to instantiate GO if isn't server
    public void RequestLootInstantiation(int prefabId, Vector3 position, LootMetaData lmd)    {
        if (IsServer) {
            InstantiateLootServerRPC(prefabId, position, lmd);
        }
        else{
            InstantiateLootServerRPCServerRpc(prefabId, position, lmd);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void InstantiateLootServerRPC(int prefabId, Vector3 pos, LootMetaData lmd){
        GameObject prefab = hm_getGO(prefabId);
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);

        // bitflag handling
        if ((lmd.lootCategory & LootCategory.LootEvent) != 0){
            ILootEvent lootEvent = prefab.GetComponent<ILootEvent>();
            if (lootEvent != null){
                lootEvent.setPlayer(/*killer*/);
                //LootEventRequierement req = lootEvent.getRequierement()
                //if(req & Requierement.data1) lootEvent.setAdditionalData1()
                //if(req & Requierement.data2) lootEvent.setAdditionalData2()
                //if(req & Requierement.data3) lootEvent.setAdditionalData3()
            }
            else{
                Debug.LogError("Error. Incoherent LootEntry. The prefab of a lootEntry with LootEvent bitflag MUST have a script component that implements ILootEvent\n"
                + "\n\tLootEntry prefab name: " + prefab.name
                + "\n\t Note that for optimization reasons, we don't check if multiples script exist, but only 1 must exist");
            }
        }
        if ((lmd.lootCategory & LootCategory.XP) != 0){
            float xpvalue = getXpByLevel(lmd.killedLevel);
            //todo : pre process xp loot ...
            //go.GetComponent<Xp>().setXpValue ...

        }

        if ((lmd.lootCategory & LootCategory.RareDrop) != 0){
            Debug.Log("Phantasmagorique ! vous avez obtenu un loot rare !");
        }

        
        go.GetComponent<NetworkObject>().Spawn();
    }

    

    [ServerRpc]
    private void InstantiateLootServerRPCServerRpc(int prefabId, Vector3 pos, LootMetaData lmd){
        InstantiateLootServerRPC(prefabId, pos, lmd);
    }
}