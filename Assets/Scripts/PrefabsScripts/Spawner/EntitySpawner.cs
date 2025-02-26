using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class EntitySpawner : PrefabSpawner
{
    [SerializeField, Range(0, 100)] private int spawnedAtlevel;
    [SerializeField] public UnityEvent<GameObject> OnIsSpawn;

    public void Initialize(int initSpawnedAtLevel, GameObject initPrefab, float initSpawnCooldown)
    {
        spawnedAtlevel = initSpawnedAtLevel;
        spawnCooldown = initSpawnCooldown;
        prefab = initPrefab;
    }

    protected override GameObject SpawnPrefabServer()
    {
        if (!LevelStateManager.Instance.SpawnEntity) return null;

        GameObject go = base.SpawnPrefabServer();

        go.GetComponent<StatistiquesLevelSystem>().CurrentLevel = spawnedAtlevel;
        go.GetComponent<HealthSystem>().RegenAllHpServerRPC();

        go.transform.SetParent(LevelStateManager.Instance.EnemyParent);

        OnIsSpawn.Invoke(go);

        return go;
    }
}
