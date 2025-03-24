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

    [SerializeField] private NetworkVariable<float> attackDamage = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public float AttackDamage
    {
        get => attackDamage.Value;
        set
        {
            if (attackDamage.Value != value)
            {
                attackDamage.Value = value;
            }
        }
    }

    [SerializeField] public UnityEvent onAttackSpeedChange;

    [SerializeField] public UnityEvent onAttackDamageChange;

    private Coroutine attackCoroutine;

    public void UpdateAttackStats()
    {
        //if (!IsServer) return;

        AttackSpeed = statsLevelSystem.CurrentStatistiques.AttackSpeed;

        AttackDamage = statsLevelSystem.CurrentStatistiques.Damage;

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
        attackDamage.OnValueChanged += OnAttackDamageChange;

        UpdateAttackStats();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        attackSpeed.OnValueChanged -= OnAttackSpeedChange;
        attackDamage.OnValueChanged -= OnAttackDamageChange;
    }

    private void OnAttackSpeedChange(float oldValue, float newValue)
    {
        onAttackSpeedChange.Invoke();
    }

    private void OnAttackDamageChange(float oldValue, float newValue)
    {
        onAttackDamageChange.Invoke();
    }

    private void OnDisable()
    {
        StopAttacks();
    }

    private void OnEnable()
    {
        RestartAttacks();
    }

    private void StartAttacks()
    {
        //if (!IsServer) return;
        attackCoroutine = StartCoroutine(LaunchAttack());
    }

    private void StopAttacks()
    {
        //if (!IsServer) return;

        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    public void RestartAttacks()
    {
        StopAttacks();

        StartAttacks();
    }

    private IEnumerator LaunchAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / (AttackSpeed * LevelStateManager.Instance.PlayerFireRateMultiplier));

            //ShootServerRPC();
            Shoot();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRPC()
    {
        if (!LevelStateManager.Instance.EnableAutoAttack) return;

        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        go.GetComponent<NetworkObject>().Spawn();
        go.transform.SetParent(LevelStateManager.Instance.ProjectileParent);
        go.GetComponent<DamageDealerSystem>().damage = AttackDamage * LevelStateManager.Instance.PlayerDamageMultiplier;
    }

    private void Shoot(){
        GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation, LevelStateManager.Instance.LocalParent);
        Projectile proj = go.GetComponent<Projectile>();
        proj.isReal = IsOwner;
        proj.damage = AttackDamage;

        //update LocalDamageSystem
    }
}