using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject prefabSpawnerEntity;
    [SerializeField] private SpawnZoneLevelDataSO spawnerZoneLevelData;
    [SerializeField] private int level = 0;
    [SerializeField] private List<BaseSpawnZoneLevelData> baseSpawnZoneLevelDatas = new List<BaseSpawnZoneLevelData>();

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        // check if client as join as owner
        if (clientId == NetworkManager.Singleton.LocalClientId || true)
        {
            UpdateSpawnerZone();
        }
    }

    public void UpdateSpawnerZone()
    {
        // update base spawn zone level datas based on level
        baseSpawnZoneLevelDatas = spawnerZoneLevelData.GetDatasOfLevel(level);

        // Clean children
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }


        // create spawners as children
        for (int i = 0; i < baseSpawnZoneLevelDatas.Count; i++)
        {
            BaseSpawnZoneLevelData baseSpawnZoneLevelData = baseSpawnZoneLevelDatas[i];

            // Instantiate the prefab as a NetworkObject
            GameObject spawner = Instantiate(prefabSpawnerEntity, transform.position, Quaternion.identity, transform);

            // Ensure the NetworkObject is spawned
            NetworkObject networkObject = spawner.GetComponent<NetworkObject>();
            networkObject.Spawn();

            // Initialize the spawner with the data
            EntitySpawner entitySpawner = spawner.GetComponent<EntitySpawner>();
            entitySpawner.Initialize(baseSpawnZoneLevelData.SpawnAtLevel, baseSpawnZoneLevelData.Prefab, baseSpawnZoneLevelData.SpawnCooldown);
        }
    }
}
