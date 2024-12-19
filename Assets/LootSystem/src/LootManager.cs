using System.Collections.Generic;
using UnityEngine;

public class LootManager
{
    // Singleton
    private static LootManager _instance;
    public static LootManager Instance => _instance ?? (_instance = new LootManager());
    private LootManager() { }

    //références vers le reste du jeu
    //private ... state;

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
    public void ProcessLootTable(LootTable lootTable, Vector3 position/*, Player killer*/)
    {
        foreach (LootEntry entry in lootTable.lootEntries)
        {
            if (DropAttempt(entry/*, killer*/))
            {
                // bitflag handling
                if ((entry.category & LootCategory.LootEvent) != 0){
                    ILootEvent lootEvent = entry.prefab.GetComponent<ILootEvent>();
                    if (lootEvent != null){
                        lootEvent.setPlayer(/*killer*/);
                        //LootEventRequierement req = lootEvent.getRequierement()
                        //if(req & Requierement.data1) lootEvent.setAdditionalData1()
                        //if(req & Requierement.data2) lootEvent.setAdditionalData2()
                        //if(req & Requierement.data3) lootEvent.setAdditionalData3()

                    }
                    else{
                        Debug.LogError("Error. Incoherent LootEntry. The prefab of a lootEntry with LootEvent bitflag MUST have a script component that implements ILootEvent\n"
                        + "\n\tLootEntry prefab name: " + entry.prefab.name
                        + "\n\tLootEntry drop chance: " + entry.dropChance
                        + "\n\t Note that for optimization reasons, we don't check if multiples script exist, but only 1 must exist");
                    }
                }
                if ((entry.category & LootCategory.RareDrop) != 0){
                    Debug.Log("Phantasmagorique ! vous avez obtenu un loot rare !, il avait genre " + entry.dropChance*100 +"% de chance d'apparaitre");
                    //todo il est très important de dire au joueur la probabilité qu'il avais de drop cette item, ça génère de la dopamine, (+formater mieux le texte)
                }
                
                GameObject.Instantiate(entry.prefab, position, Quaternion.identity);
            }
        }
    }
}
