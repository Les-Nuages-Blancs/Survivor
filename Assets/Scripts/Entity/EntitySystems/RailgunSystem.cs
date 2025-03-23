using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RailgunSystem : NetworkBehaviour
{
    public int level = 1; //plug upgrades here
    public float cooldown = 5;
    [Tooltip("warning : must match the visual lenght of railgun vfx !")] //doing it manually would be a waste at runtime. Unless the lenghts can change at runtime then another logic would be responsible for controlling the lenght of the vfx and this dmg dealing script
    [SerializeField] private float length = 18;
    [SerializeField] private float delayBeforeDamage = 0.25f;
    [SerializeField] private AnimationCurve DmgPerLevel;
    [SerializeField] private GameObject railgunPrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    private bool blockShoot = false; //used to block firing without desyncronising fire timing between players


    private Coroutine railgunCoroutine;

    //called by death system
    public void ToggleAlive(){
        blockShoot = !blockShoot;
    }

    private void StartAttacks(){
        railgunCoroutine = StartCoroutine(LaunchAttack());
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown /*plug cheat here ?*/);
            if(blockShoot) continue;
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
        railgun.length = length;
        railgun.delayBeforeDamage = delayBeforeDamage;
        railgun.damage = DmgPerLevel.Evaluate(level);

        //update LocalDamageSystem
    }
}
