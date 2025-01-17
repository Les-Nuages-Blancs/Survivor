using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnZoneLevelStatistiques", menuName = "ScriptableObject/SpawnZoneLevelStatistiques")]
public class SpawnZoneLevelDataSO : ScriptableObject
{
    [SerializeField] public List<BaseSpawnZoneLevelDataGroup> levelDatas = new List<BaseSpawnZoneLevelDataGroup> { new BaseSpawnZoneLevelDataGroup() };

    public List<BaseSpawnZoneLevelData> GetDatasOfLevel(int levelIndex)
    {
        List<BaseSpawnZoneLevelData> baseSpawnZoneLevelDatas = new List<BaseSpawnZoneLevelData>();
        foreach (var levelData in levelDatas)
        {
            baseSpawnZoneLevelDatas.Add(levelData.baseSpawnZoneLevelDatas[levelIndex].Clone());
        }
        return baseSpawnZoneLevelDatas;
    }
}
