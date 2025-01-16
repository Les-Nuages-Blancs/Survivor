using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.Events;

public class AutoAttackSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private NetworkVariable<float> attackSpeed = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public float AttackSpeed
    {
        get => attackSpeed.Value;
        set
        {
            if (attackSpeed.Value != value)
            {
                attackSpeed.Value = value;
            }
        }
    }

    [SerializeField] public UnityEvent onAttackSpeedChange;

    private Coroutine attackCoroutine;

    public void UpdateAttackStats()
    {
        if (!IsServer) return;

        AttackSpeed = statsLevelSystem.BaseStatistiques.AttackSpeed;

        if (Application.isPlaying )
        {
            RestartAttacks();
        }
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
        attackSpeed.OnValueChanged += OnAttackSpeedChange;

        UpdateAttackStats();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        attackSpeed.OnValueChanged -= OnAttackSpeedChange;
    }

    private void OnAttackSpeedChange(float oldValue, float newValue)
    {
        onAttackSpeedChange.Invoke();
    }

    private void OnDisable()
    {
        StopAttacks();
    }

    private void StartAttacks()
    {
        if (!IsServer) return;
        attackCoroutine = StartCoroutine(LaunchAttack());
    }

    private void StopAttacks()
    {
        if (!IsServer) return;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private void RestartAttacks()
    {
        StopAttacks();

        StartAttacks();
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / AttackSpeed);

            ShootServerRPC();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRPC()
    {
        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        go.GetComponent<NetworkObject>().Spawn();
    }
}