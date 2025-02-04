using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UIElements;
//using System.Numerics;

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

    [SerializeField] public float predictionFac = 1.5f; // Litteraly a magic value that depends on server tickrate
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

            ShootServerRPC(NetworkManager.Singleton.LocalClientId, true);
            //ShootServerRPC(NetworkManager.Singleton.LocalClientId, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRPC(ulong clientId, bool predict) //, Vector3 pos, Quaternion rot)
    {
        if (!LevelStateManager.Instance.EnableAutoAttack) return;

        Vector3 ppos = projectileSpawnPoint.position; //proj possition
        Vector3 predictedPos = ppos;

        if(predict){ //won't work because the server handles all auto attacks and for some reason The server can't access the simple movement system for client character
            Debug.Log("coucou");
            //GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            Vector3 targetPos = GetComponent<SimpleMovementSystem>().targetPosition ?? ppos + new Vector3(0,2,0); //debug y up to show issue
            Vector3 offset = targetPos - transform.position;
            Vector3 predictedDir = Vector3.Normalize(offset);
            

            //var currentPosition = transform.position;
            //transform.position = Vector3.MoveTowards(currentPosition, targetPosition.Value, MoveSpeed * Time.fixedDeltaTime);
            predictedPos +=  predictedDir * predictionFac;
        }

        //Debug.Log($"actual pos {ppos} / predicted pos {predictedPos}");


        //GameObject go = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        GameObject go = Instantiate(projectilePrefab, predictedPos, projectileSpawnPoint.rotation);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    }
}