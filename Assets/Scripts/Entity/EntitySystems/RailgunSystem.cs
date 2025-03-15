using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RailgunSystem : NetworkBehaviour
{
    public int level = 1; //plug upgrades here
    public float cooldown = 5;
    [SerializeField] private AnimationCurve DmgPerLevel;
    [SerializeField] private GameObject railgunPrefab;
    [SerializeField] private Transform projectileSpawnPoint;


    private Coroutine railgunCoroutine;

    private void StartAttacks(){
        railgunCoroutine = StartCoroutine(LaunchAttack());
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown /*plug cheat here ?*/); 
            if(level > 0) Shoot();
        }
    }

    // Start is called before the first frame update
    void Start(){
        StartAttacks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Shoot(){
        GameObject go = Instantiate(railgunPrefab, projectileSpawnPoint.position, transform.rotation, LevelStateManager.Instance.LocalParent);
        Railgun railgun = go.GetComponent<Railgun>();
        railgun.isReal = IsOwner;
        railgun.damage = DmgPerLevel.Evaluate(level);

        //update LocalDamageSystem
    }
}
