using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;

public class SpawnerZone : NetworkBehaviour
{
    [SerializeField] private List<SpawnCondition> conditions;
    [SerializeField] private GameObject prefabSpawnerEntity;
    [SerializeField] private SpawnZoneLevelDataSO spawnerZoneLevelData;
    [SerializeField] private List<BaseSpawnZoneLevelData> baseSpawnZoneLevelDatas = new List<BaseSpawnZoneLevelData>();
    private List<GameObject> spawnedSpawner = new List<GameObject>();
    private bool levelMaxIsReached = false;
    public Zone ParentZone;

    [SerializeField] private NetworkVariable<int> playerZoneLevel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public int PlayerZoneLevel
    {
        get => playerZoneLevel.Value;
        set
        {
            if (playerZoneLevel.Value != value)
            {
                playerZoneLevel.Value = value;

                UpdateSpawnerZone();
            }
        }
    }

    [SerializeField] public UnityEvent onPlayerZoneLevelChange;
    [SerializeField] public UnityEvent onLevelMaxReached;

    public void AddCondition(SpawnCondition condition)
    {
        conditions.Add(condition);
        foreach (GameObject spawnerGo in spawnedSpawner)
        {
            spawnerGo.GetComponent<EntitySpawner>().spawnConditions.Add(condition);
        }
    }

    public override void OnNetworkSpawn()
    {
        playerZoneLevel.OnValueChanged += OnPlayerZoneLevelChange;

        if (IsOwner)
        {
            UpdateSpawnerZone();
        }
    }

    public void LevelUp()
    {
        if (levelMaxIsReached) return;

        int maxLevel = spawnerZoneLevelData.levelDatas[0].baseSpawnZoneLevelDatas.Count - 1;

        playerZoneLevel.Value += 1;

        bool levelMaxReached = PlayerZoneLevel > maxLevel;

        if (levelMaxReached)
        {
            playerZoneLevel.Value = Mathf.Min(playerZoneLevel.Value, maxLevel);
        }

        UpdateSpawnerZone();

        if (levelMaxReached) 
        {
            levelMaxIsReached = true;

            onLevelMaxReached.Invoke();
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        playerZoneLevel.OnValueChanged -= OnPlayerZoneLevelChange;
    }

    private void OnPlayerZoneLevelChange(int oldValue, int newValue)
    {
        onPlayerZoneLevelChange.Invoke();
        Debug.Log("level up ! " + gameObject.name);
    }

    public void UpdateSpawnerZone()
    {
        if (!IsOwner) return;

        UpdateSpawnerZoneServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc]
    public void UpdateSpawnerZoneServerRpc(ulong localClientId)
    {
        // update base spawn zone level datas based on level
        baseSpawnZoneLevelDatas = spawnerZoneLevelData.GetDatasOfLevel(playerZoneLevel.Value);

        // Clean children
        DestroyEntitySpawner();

        // create spawners as children
        for (int i = 0; i < baseSpawnZoneLevelDatas.Count; i++)
        {
            BaseSpawnZoneLevelData baseSpawnZoneLevelData = baseSpawnZoneLevelDatas[i];

            // Instantiate the prefab as a NetworkObject
            GameObject spawner = Instantiate(prefabSpawnerEntity, transform.position, Quaternion.identity, transform);
            spawnedSpawner.Add(spawner);

            // Ensure the NetworkObject is spawned
            NetworkObject networkObjectTarget = spawner.GetComponent<NetworkObject>();
            networkObjectTarget.SpawnWithOwnership(localClientId);
            networkObjectTarget.transform.SetParent(transform);


            // Initialize the spawner with the data
            EntitySpawner entitySpawner = spawner.GetComponent<EntitySpawner>();
            entitySpawner.Initialize(baseSpawnZoneLevelData.SpawnAtLevel, baseSpawnZoneLevelData.Prefab, baseSpawnZoneLevelData.SpawnCooldown);
            entitySpawner.OnIsSpawn.AddListener(RegisterEnemy);
            // add condition;
            Debug.Log("add spawnerZone condition to spawner");

            foreach (SpawnCondition condition in conditions)
            {
                entitySpawner.spawnConditions.Add(condition);
            }
        }
    }

    public void RegisterEnemy(GameObject go)
    {
        ParentZone.EnemyCount += 1;
        go.GetComponent<HealthSystem>().onDeath.AddListener(() => UnregisterEnemy(go));
    }

    public void UnregisterEnemy(GameObject go)
    {
        ParentZone.EnemyCount -= 1;
    }

    private void DestroyEntitySpawner()
    {
        while (spawnedSpawner.Count > 0) 
        {
            GameObject spawner = spawnedSpawner[0];
            spawnedSpawner.Remove(spawner);
            spawner.GetComponent<NetworkObject>().Despawn();

            EntitySpawner entitySpawner = spawner.GetComponent<EntitySpawner>();
            entitySpawner.OnIsSpawn.RemoveListener(RegisterEnemy);
            
            Destroy(spawner);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        DestroyEntitySpawner();
    }
}
