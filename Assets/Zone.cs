using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Zone : NetworkBehaviour
{
    [SerializeField] private List<SpawnCondition> conditions;
    [SerializeField] private string zoneName = "default name";

    [SerializeField] private GameObject prefabPlayerSpawner;
    [SerializeField] private int MaxEnemyCount = 100;
    [SerializeField] private bool isUnlock = false;

    [SerializeField] private List<TaskZone> zoneTasks = new List<TaskZone>();

    [SerializeField] public UnityEvent<TaskZone> OnTaskAdded;
    [SerializeField] public UnityEvent<TaskZone> OnTaskRemove;

    private Dictionary<ulong, SpawnerZone> playerSpawners = new Dictionary<ulong, SpawnerZone>();

    public List<TaskZone> ZoneTasks => zoneTasks; // Read-only access to prevent direct modification
    public string ZoneName => zoneName; // Read-only access to prevent direct modification
    public Dictionary<ulong, SpawnerZone> PlayerSpawners => playerSpawners; // Read-only access to prevent direct modification

    public void AddTask(TaskZone task)
    {
        if (!zoneTasks.Contains(task))
        {
            zoneTasks.Add(task);
            OnTaskAdded?.Invoke(task);
        }
    }

    public void RemoveTask(TaskZone task)
    {
        if (zoneTasks.Remove(task)) // Returns true if removed
        {
            OnTaskRemove?.Invoke(task);
        }
    }

    private List<GameObject> spawnedPrefabs = new List<GameObject>();
    private int enemyCount = 0;

    public int EnemyCount
    {
        get => enemyCount;
        set
        {
            if (enemyCount != value)
            {
                int difference = value - enemyCount;
                LevelStateManager.Instance.totalNumberOfEnemy += difference;

                enemyCount = value;
                onEnemyCountChange.Invoke();
            }
        }
    }

    public bool IsUnlock
    {
        get => isUnlock;
        set
        {
            if (isUnlock != value)
            {
                isUnlock = value;
                onUnlock.Invoke();
            }
        }
    }

    [SerializeField] private UnityEvent onEnemyCountChange;
    [SerializeField] public UnityEvent onUnlock;

    public bool CanSpawnEnemyInZone()
    {
        return EnemyCount < MaxEnemyCount;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += AddEnemySpawnerForNewPlayer;
        }
    }

    public void AddEnemySpawnerForNewPlayer(ulong clientId)
    {
        GameObject newSpawnedPrefab = Instantiate(prefabPlayerSpawner, transform.position, transform.rotation);
        spawnedPrefabs.Add(newSpawnedPrefab);

        NetworkObject networkObjectTarget = newSpawnedPrefab.GetComponent<NetworkObject>();
        networkObjectTarget.SpawnWithOwnership(clientId);

        newSpawnedPrefab.transform.SetParent(transform);
        newSpawnedPrefab.GetComponent<OwnerZonePresence>().Zone = this;

        SpawnerZone spawnerZone = newSpawnedPrefab.GetComponent<SpawnerZone>();
        spawnerZone.ParentZone = this;

        foreach (SpawnCondition condition in conditions)
        {
            spawnerZone.AddCondition(condition);
        }

        playerSpawners[clientId] = spawnerZone;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= AddEnemySpawnerForNewPlayer;
        }

        if (!IsOwner) return;

        foreach (GameObject spawnedPrefab in spawnedPrefabs)
        {  
            spawnedPrefab.GetComponent<NetworkObject>().Despawn();
            Destroy(spawnedPrefab);
        }
    }

    public Vector3 GetRandomPositionOnNavMesh(Transform originTransform, float minDistance = 5.0f, float maxDistance = 10.0f)
    {
        while (true)
        {
            // Generate a random point within a disk
            float randomAngle = Random.Range(0, 2 * Mathf.PI); // Random angle in radians
            float randomRadius = Mathf.Sqrt(Random.Range(minDistance * minDistance, maxDistance * maxDistance)); // Random radius within range
            Vector3 randomPoint = new Vector3(
                Mathf.Cos(randomAngle) * randomRadius,
                0,
                Mathf.Sin(randomAngle) * randomRadius
            );

            randomPoint += originTransform.position; // Offset from the object's position

            NavMeshHit hit;
            // Check if the point is on the NavMesh
            if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, NavMesh.AllAreas))
            {
                float distanceToPoint = Vector3.Distance(originTransform.position, hit.position);
                if (distanceToPoint >= minDistance && distanceToPoint <= maxDistance)
                {
                    return hit.position; // Valid point found
                }
            }
        }
    }
}
