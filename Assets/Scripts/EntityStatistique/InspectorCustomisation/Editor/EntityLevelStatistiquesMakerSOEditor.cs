using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EntityLevelStatistiquesMakerSO))]
public class EntityLevelStatistiquesMakerSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EntityLevelStatistiquesMakerSO script = (EntityLevelStatistiquesMakerSO)target;

        // Show the "Create Generated Stats Asset" button only if the generatedEntityLevelStats is null
        if (script.generatedEntityLevelStats == null)
        {
            if (GUILayout.Button("Create Generated Stats Asset"))
            {
                script.CreateGeneratedStatsAsset();
            }
        }
        else
        {
            // Show the "Update Level Stats" button if generatedEntityLevelStats is not null
            if (GUILayout.Button("Update Level Stats"))
            {
                script.UpdateLevelStatistiques();
            }
        }
    }
}
