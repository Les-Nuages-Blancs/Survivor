using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Zone : NetworkBehaviour
{
    [SerializeField] private GameObject prefabPlayerSpawner;
    private GameObject spawnedPrefab;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        spawnedPrefab = Instantiate(prefabPlayerSpawner, transform.position, transform.rotation);
        spawnedPrefab.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        spawnedPrefab.GetComponent<NetworkObject>().Despawn();
        Destroy(spawnedPrefab);
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
