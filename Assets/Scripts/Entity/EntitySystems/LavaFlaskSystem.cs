using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LavaFlaskSystem : NetworkBehaviour
{
    public int level = 1; //plug upgrades here
    public float cooldown = 5;
    [SerializeField] private AnimationCurve DmgPerLevel;
    [Tooltip("the size of the lava flask")]
    [SerializeField] private AnimationCurve sizePerLevel;
    [Tooltip("How many times the lava will tick and deal dmg")]
    public int tickCount = 3;
    [Tooltip("Delay in second between dmg ticks")]
    public float tickDelay = 1.0f;


    [SerializeField] private GameObject lavaflaskPrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    private bool blockShoot = false; //used to block firing without desyncronising fire timing between players
    private Coroutine railgunCoroutine;


    public void ToggleAlive(){
        blockShoot = !blockShoot;
    }

    public void StartAttacks(){
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
        //StartAttacks(); called when the game starts
    }


    private void Shoot(){
        GameObject go = Instantiate(lavaflaskPrefab, projectileSpawnPoint.position, Quaternion.identity, LevelStateManager.Instance.LocalParent);
        LavaFlask flask = go.GetComponent<LavaFlask>();
        flask.isReal = IsOwner;
        flask.damage = DmgPerLevel.Evaluate(level);
        flask.size = sizePerLevel.Evaluate(level);
        flask.tickCount = tickCount;
        flask.tickDelay = tickDelay;
        //todo size and tick
        flask.damage = DmgPerLevel.Evaluate(level);
    }
}
