using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnZoneLevelDataMakerSO))]
public class SpawnZoneLevelDataMakerSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnZoneLevelDataMakerSO script = (SpawnZoneLevelDataMakerSO)target;

        // Show the "Create Generated Datas Asset" button only if the generatedSpawnZoneLevelDatas is null
        if (script.generatedSpawnZoneLevelDatas == null)
        {
            if (GUILayout.Button("Create Generated Datas Asset"))
            {
                script.CreateGeneratedDatasAsset();
            }
        }
        else
        {
            // Show the "Update Level Datas" button if generatedSpawnZoneLevelDatas is not null
            if (GUILayout.Button("Update Level Datas"))
            {
                script.UpdateLevelDatas();
            }
        }
    }
}
