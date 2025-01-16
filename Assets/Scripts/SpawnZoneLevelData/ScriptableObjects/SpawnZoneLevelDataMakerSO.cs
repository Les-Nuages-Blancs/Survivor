#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnZoneLevelStatistiquesMaker", menuName = "ScriptableObject/SpawnZoneLevelStatistiquesMaker")]
public class SpawnZoneLevelDataMakerSO : ScriptableObject
{
    [Header("Stats Maker Settings")]
    [SerializeField] private bool updateOnChange = true;

    [Header("Stats Maker Inputs")]
    [SerializeField] private BaseSpawnZoneLevelData baseSpawnZoneLevelData = new BaseSpawnZoneLevelData();
    [SerializeField]
    private List<SpawnZoneLevelRangeUpgrader> levelRangeUpgrader = new List<SpawnZoneLevelRangeUpgrader>
    {
        new SpawnZoneLevelRangeUpgrader()
    };

    [Header("Stats Maker Outputs")]
    [SerializeField] public SpawnZoneLevelDataSO generatedSpawnZoneLevelDatas;

    // Method to create the datas asset
    public void CreateGeneratedDatasAsset()
    {
        if (generatedSpawnZoneLevelDatas == null)
        {
            string basePath = "Assets/ScriptableObjects/SpawnZoneLevelData/GeneratedSpawnZoneLevelStats";
            string extension = ".asset";
            string assetPath = basePath + extension;

            // Check if the asset already exists, and if so, increment the number until an available path is found
            int counter = 1;
            while (AssetDatabase.AssetPathToGUID(assetPath) != "")
            {
                assetPath = basePath + counter + extension;
                counter++;
            }

            // Create a new instance and save it at the unique path
            generatedSpawnZoneLevelDatas = ScriptableObject.CreateInstance<SpawnZoneLevelDataSO>();
            AssetDatabase.CreateAsset(generatedSpawnZoneLevelDatas, assetPath);
            AssetDatabase.SaveAssets();
        }

        if (updateOnChange)
        {
            UpdateLevelDatas();
        }
    }

    // Method to update the datas
    public void UpdateLevelDatas()
    {
        if (generatedSpawnZoneLevelDatas != null)
        {
            generatedSpawnZoneLevelDatas.levelDatas.Clear();
            generatedSpawnZoneLevelDatas.levelDatas.Add(baseSpawnZoneLevelData.Clone());
            BaseSpawnZoneLevelData currentStats = baseSpawnZoneLevelData;

            foreach (SpawnZoneLevelRangeUpgrader entityLevelRangeUpgrader in levelRangeUpgrader)
            {
                for (int i = 0; i < entityLevelRangeUpgrader.NumberOfLevel; i++)
                {
                    foreach (SpawnZoneLevelUpgraderMode entityLevelUpgraderMode in entityLevelRangeUpgrader.LevelUpgrader.EntityLevelUpgraderModes)
                    {
                        BaseSpawnZoneLevelData upgradedStats = entityLevelUpgraderMode.ApplyOperation(baseSpawnZoneLevelData, currentStats);
                        generatedSpawnZoneLevelDatas.levelDatas.Add(upgradedStats);
                        currentStats = upgradedStats;
                    }
                }
            }
        }
    }

    // Automatically trigger updates in the Editor (if enabled)
    private void OnValidate()
    {
        if (updateOnChange)
        {
            UpdateLevelDatas();
        }
    }
}
#endif