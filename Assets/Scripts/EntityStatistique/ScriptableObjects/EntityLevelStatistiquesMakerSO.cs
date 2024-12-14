#if UNITY_EDITOR
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityLevelStatistiquesMaker", menuName = "ScriptableObject/EntityLevelStatistiquesMaker")]
public class EntityLevelStatistiquesMakerSO : ScriptableObject
{
    [Header("Stats Maker Settings")]
    [SerializeField] private bool updateOnChange = true;

    [Header("Stats Maker Inputs")]
    [SerializeField] private EntityBaseStatistiques baseStatistiques = new EntityBaseStatistiques(100, 100.0f, 1.0f, 20.0f, 15.0f, 2.0f, 0.2f, 0.05f, 5.0f, 2.0f);
    [SerializeField]
    private List<EntityLevelRangeUpgrader> levelRangeUpgrader = new List<EntityLevelRangeUpgrader>
    {
        new EntityLevelRangeUpgrader()
    };

    [Header("Stats Maker Outputs")]
    [SerializeField] public EntityLevelStatistiquesSO generatedEntityLevelStats;

    // Method to create the stats asset
    public void CreateGeneratedStatsAsset()
    {
        if (generatedEntityLevelStats == null)
        {
            string basePath = "Assets/ScriptableObjects/Statistiques/GeneratedEntityLevelStats";
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
            generatedEntityLevelStats = ScriptableObject.CreateInstance<EntityLevelStatistiquesSO>();
            AssetDatabase.CreateAsset(generatedEntityLevelStats, assetPath);
            AssetDatabase.SaveAssets();
        }

        if (updateOnChange)
        {
            UpdateLevelStatistiques();
        }
    }

    // Method to update the stats
    public void UpdateLevelStatistiques()
    {
        if (generatedEntityLevelStats != null)
        {
            generatedEntityLevelStats.levelStatistiques.Clear();
            generatedEntityLevelStats.levelStatistiques.Add(baseStatistiques.Clone());
            EntityBaseStatistiques currentStats = baseStatistiques;

            foreach (EntityLevelRangeUpgrader entityLevelRangeUpgrader in levelRangeUpgrader)
            {
                for (int i = 0; i < entityLevelRangeUpgrader.NumberOfLevel; i++)
                {
                    foreach (EntityLevelUpgraderMode entityLevelUpgraderMode in entityLevelRangeUpgrader.LevelUpgrader.EntityLevelUpgraderModes)
                    {
                        EntityBaseStatistiques upgradedStats = entityLevelUpgraderMode.ApplyOperation(baseStatistiques, currentStats);
                        generatedEntityLevelStats.levelStatistiques.Add(upgradedStats);
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
            UpdateLevelStatistiques();
        }
    }
}
#endif