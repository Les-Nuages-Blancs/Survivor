using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Zone : NetworkBehaviour
{
    [SerializeField] private GameObject prefabPlayerSpawner;

    private List<GameObject> spawnedPrefabs = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            Debug.Log("subscribe here <----------- XoXo");

            NetworkManager.Singleton.OnClientConnectedCallback += AddEnemySpawnerForNewPlayer;
        }
    }

    public void AddEnemySpawnerForNewPlayer(ulong clientId)
    {
        Debug.Log("new connexion " + clientId);

        GameObject newSpawnedPrefab = Instantiate(prefabPlayerSpawner, transform.position, transform.rotation);
        spawnedPrefabs.Add(newSpawnedPrefab);

        NetworkObject networkObjectTarget = newSpawnedPrefab.GetComponent<NetworkObject>();
        networkObjectTarget.SpawnWithOwnership(clientId);
        Debug.Log(" -> " + clientId);

        newSpawnedPrefab.transform.SetParent(transform);
        newSpawnedPrefab.GetComponent<OwnerZonePresence>().Zone = this;
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
