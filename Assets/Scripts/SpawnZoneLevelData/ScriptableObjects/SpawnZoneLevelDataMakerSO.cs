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
    [SerializeField] private List<BaseSpawnZoneLevelData> baseSpawnZoneLevelData = new List<BaseSpawnZoneLevelData> { 
        new BaseSpawnZoneLevelData() 
    };

    [SerializeField]
    private List<SpawnZoneLevelRangeGroup> levelRangeUpgrader = new List<SpawnZoneLevelRangeGroup> {
        new SpawnZoneLevelRangeGroup()
    };

    [Header("Stats Maker Outputs")]
    [SerializeField] public SpawnZoneLevelDataSO generatedSpawnZoneLevelDatas;

    // Method to create the datas asset
    public void CreateGeneratedDatasAsset()
    {
        if (generatedSpawnZoneLevelDatas == null)
        {
            string basePath = "Assets/ScriptableObjects/SpawnZoneLevelData/SpawnZoneLevelData/GeneratedSpawnZoneLevelStats";
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

            for (int i = 0; i < levelRangeUpgrader.Count; i++)
            {
                List<SpawnZoneLevelRangeUpgrader> listOfSpawnZoneLevelRangeUpgrader = levelRangeUpgrader[i].levelRangeUpgraders;

                BaseSpawnZoneLevelDataGroup levelDatas = new BaseSpawnZoneLevelDataGroup();
                levelDatas.baseSpawnZoneLevelDatas.Add(baseSpawnZoneLevelData[i].Clone());

                BaseSpawnZoneLevelData currentStats = baseSpawnZoneLevelData[i];

                foreach (SpawnZoneLevelRangeUpgrader spawnZoneLevelRangeUpgrader in listOfSpawnZoneLevelRangeUpgrader)
                {

                    for (int j = 0; j < spawnZoneLevelRangeUpgrader.NumberOfLevel; j++)
                    {
                        foreach (SpawnZoneLevelUpgraderMode spawnZoneLevelRangeUpgraderMode in spawnZoneLevelRangeUpgrader.LevelUpgrader.SpawnZoneLevelUpgraderModes)
                        {
                            BaseSpawnZoneLevelData upgradedStats = spawnZoneLevelRangeUpgraderMode.ApplyOperation(baseSpawnZoneLevelData[i], currentStats);
                            currentStats = upgradedStats;
                        }
                        levelDatas.baseSpawnZoneLevelDatas.Add(currentStats);
                    }
                    
                }
                generatedSpawnZoneLevelDatas.levelDatas.Add(levelDatas);
            }

            // Mark the ScriptableObject as dirty so Unity knows it has changed
            EditorUtility.SetDirty(generatedSpawnZoneLevelDatas);
            AssetDatabase.SaveAssets();  // Ensure it's saved to disk
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