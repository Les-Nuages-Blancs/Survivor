using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;

public class SpawnerZone : NetworkBehaviour
{
    [SerializeField] private SpawnCondition condition;
    [SerializeField] private GameObject prefabSpawnerEntity;
    [SerializeField] private SpawnZoneLevelDataSO spawnerZoneLevelData;
    [SerializeField] private List<BaseSpawnZoneLevelData> baseSpawnZoneLevelDatas = new List<BaseSpawnZoneLevelData>();
    private List<GameObject> spawnedSpawner = new List<GameObject>();

    [SerializeField] private NetworkVariable<int> playerZoneLevel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

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
    }

    public void UpdateSpawnerZone()
    {
        // update base spawn zone level datas based on level
        baseSpawnZoneLevelDatas = spawnerZoneLevelData.GetDatasOfLevel(playerZoneLevel.Value);

        if (!IsOwner) return;

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
            NetworkObject networkObject = spawner.GetComponent<NetworkObject>();
            networkObject.Spawn();

            // Initialize the spawner with the data
            EntitySpawner entitySpawner = spawner.GetComponent<EntitySpawner>();
            entitySpawner.Initialize(baseSpawnZoneLevelData.SpawnAtLevel, baseSpawnZoneLevelData.Prefab, baseSpawnZoneLevelData.SpawnCooldown);

            // add is inside condition;
            entitySpawner.spawnConditions.Add(condition);
        }
    }

    private void DestroyEntitySpawner()
    {
        while (spawnedSpawner.Count > 0) 
        {
            GameObject spawner = spawnedSpawner[0];
            spawnedSpawner.Remove(spawner);
            spawner.GetComponent<NetworkObject>().Despawn();
            Destroy(spawner);
        }
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        DestroyEntitySpawner();
    }
}
