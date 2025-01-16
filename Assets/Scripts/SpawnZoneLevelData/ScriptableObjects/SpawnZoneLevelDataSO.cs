using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnZoneLevelStatistiques", menuName = "ScriptableObject/SpawnZoneLevelStatistiques")]
public class SpawnZoneLevelDataSO : ScriptableObject
{
    [SpawnZoneLevel]
    [SerializeField] public List<BaseSpawnZoneLevelData> levelDatas = new List<BaseSpawnZoneLevelData>();

    public BaseSpawnZoneLevelData GetDatasOfLevel(int levelIndex)
    {
        return levelDatas[levelIndex].Clone();
    }
}
