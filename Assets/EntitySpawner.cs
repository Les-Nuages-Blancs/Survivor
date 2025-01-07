using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EntitySpawner : PrefabSpawner
{
    [SerializeField, Range(0, 100)] private int spawnedAtlevel;

    protected override GameObject SpawnPrefabServer()
    {
        GameObject go = base.SpawnPrefabServer();

        go.GetComponent<StatistiquesLevelSystem>().CurrentLevel = spawnedAtlevel;
        go.GetComponent<HealthSystem>().RegenAllHpServerRPC();

        return go;
    }

}
