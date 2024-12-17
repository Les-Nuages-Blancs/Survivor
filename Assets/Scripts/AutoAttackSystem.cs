using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System.Collections.Generic;

public class AutoAttackSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float attackSpeed;

    private Coroutine attackCoroutine;

    private void Start()
    {
        UpdateAttackStats();
    }

    public void UpdateAttackStats()
    {
        attackSpeed = statsLevelSystem.BaseStatistiques.AttackSpeed;
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateAttackStats();
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        attackCoroutine = StartCoroutine(LaunchAttack());
    }

    private void OnDisable()
    {
        if (!IsOwner) return;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / attackSpeed);

            ShootServerRPC();
        }
    }

    [ServerRpc]
    private void ShootServerRPC()
    {
        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}