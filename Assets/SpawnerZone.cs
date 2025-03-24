using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SpawnerZone : NetworkBehaviour
{
    [SerializeField] private List<SpawnCondition> conditions;
    [SerializeField] private GameObject prefabSpawnerEntity;
    [SerializeField] private SpawnZoneLevelDataSO spawnerZoneLevelData;
    [SerializeField] private List<BaseSpawnZoneLevelData> baseSpawnZoneLevelDatas = new List<BaseSpawnZoneLevelData>();
    private List<GameObject> spawnedSpawner = new List<GameObject>();
    private bool levelMaxIsReached = false;

    [SerializeField]
    private NetworkVariable<NetworkObjectReference> parentZoneRef =
        new NetworkVariable<NetworkObjectReference>(new NetworkObjectReference(), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [SerializeField] public UnityEvent<Zone, Zone> OnParentZoneChanged;
    [SerializeField] public UnityEvent onPlayerZoneLevelChange;
    [SerializeField] public UnityEvent onLevelMaxReached;

    public SpawnZoneLevelDataSO SpawnerZoneLevelData => spawnerZoneLevelData;

    public Zone ParentZone
    {
        get
        {
            if (parentZoneRef.Value.TryGet(out NetworkObject parentObj))
            {
                return parentObj.GetComponent<Zone>();
            }
            return null;
        }
        set
        {
            if (value != ParentZone)
            {
                Zone oldZone = ParentZone;
                parentZoneRef.Value = new NetworkObjectReference(value.GetComponent<NetworkObject>());
                OnParentZoneChanged.Invoke(oldZone, value);
            }
        }
    }

    [SerializeField]
    private NetworkVariable<int> playerZoneLevel =
        new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

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
        parentZoneRef.OnValueChanged += OnParentZoneChange;

        if (IsOwner)
        {
            UpdateSpawnerZone();
        }
    }

    private void OnParentZoneChange(NetworkObjectReference oldRef, NetworkObjectReference newRef)
    {
        OnParentZoneChanged.Invoke(oldRef.TryGet(out NetworkObject oldObj) ? oldObj.GetComponent<Zone>() : null,
                                   newRef.TryGet(out NetworkObject newObj) ? newObj.GetComponent<Zone>() : null);
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
        parentZoneRef.OnValueChanged -= OnParentZoneChange;
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
        baseSpawnZoneLevelDatas = spawnerZoneLevelData.GetDatasOfLevel(playerZoneLevel.Value);

        DestroyEntitySpawner();

        for (int i = 0; i < baseSpawnZoneLevelDatas.Count; i++)
        {
            BaseSpawnZoneLevelData baseSpawnZoneLevelData = baseSpawnZoneLevelDatas[i];

            GameObject spawner = Instantiate(prefabSpawnerEntity, transform.position, Quaternion.identity, transform);
            spawnedSpawner.Add(spawner);

            NetworkObject networkObjectTarget = spawner.GetComponent<NetworkObject>();
            networkObjectTarget.SpawnWithOwnership(localClientId);
            networkObjectTarget.transform.SetParent(transform);

            EntitySpawner entitySpawner = spawner.GetComponent<EntitySpawner>();
            entitySpawner.Initialize(baseSpawnZoneLevelData.SpawnAtLevel, baseSpawnZoneLevelData.Prefab, baseSpawnZoneLevelData.SpawnCooldown);
            entitySpawner.OnIsSpawn.AddListener(RegisterEnemy);

            foreach (SpawnCondition condition in conditions)
            {
                entitySpawner.spawnConditions.Add(condition);
            }
        }
    }

    public void RegisterEnemy(GameObject go)
    {
        if (ParentZone != null)
        {
            ParentZone.EnemyCount += 1;
            go.GetComponent<HealthSystem>().onDeath.AddListener(() => UnregisterEnemy(go));
        }
    }

    public void UnregisterEnemy(GameObject go)
    {
        if (ParentZone != null)
        {
            ParentZone.EnemyCount -= 1;
        }
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
