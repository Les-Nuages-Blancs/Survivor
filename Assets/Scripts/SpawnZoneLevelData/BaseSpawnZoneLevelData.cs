using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseSpawnZoneLevelData
{
    [SerializeField] private GameObject prefab;
    [SerializeField, Range(0.0f, 100.0f)] private float probability = 100.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float spawnCooldown = 2.0f;
    [SerializeField, Range(0, 100)] private int spawnAtLevel = 0;

    public GameObject Prefab => prefab;
    public float Probability => probability;
    public float SpawnCooldown => spawnCooldown;
    public int SpawnAtLevel => spawnAtLevel;

    public BaseSpawnZoneLevelData()
    {
        probability = 100.0f;
        spawnCooldown = 2.0f;
        spawnAtLevel = 0;
    }

    public BaseSpawnZoneLevelData(
        float initProbability = 100.0f,
        float initSpawnCooldown = 2.0f,
        int initSpawnAtLevel = 0
    )
    {
        probability = initProbability;
        spawnCooldown = initSpawnCooldown;
        spawnAtLevel = initSpawnAtLevel;
    }

    // Addition operator
    public static BaseSpawnZoneLevelData operator +(BaseSpawnZoneLevelData a, BaseSpawnZoneLevelData b)
    {
        return new BaseSpawnZoneLevelData(
            a.probability + b.probability,
            a.spawnCooldown + b.spawnCooldown,
            a.spawnAtLevel + b.spawnAtLevel
        );
    }

    // Multiplication operator
    public static BaseSpawnZoneLevelData operator *(BaseSpawnZoneLevelData a, BaseSpawnZoneLevelData b)
    {
        return new BaseSpawnZoneLevelData(
            a.probability * b.probability,
            a.spawnCooldown * b.spawnCooldown,
            a.spawnAtLevel * b.spawnAtLevel
        );
    }

    // Clone method
    public BaseSpawnZoneLevelData Clone()
    {
        return new BaseSpawnZoneLevelData(
            probability,
            spawnCooldown,
            spawnAtLevel
        );
    }
}
