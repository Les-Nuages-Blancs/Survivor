#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatistiquesLevelSystem))]
public class StatistiquesLevelSystemEditor : Editor
{
    // Variables to hold parameter values for the buttons
    private int addLevel = 1;
    private int addXp = 0;

    // Store references to StatistiquesLevelSystem to listen for changes
    private StatistiquesLevelSystem levelSystem;

    // TODO allow set level in editor when application is not running and save this data

    private void OnEnable()
    {
        // Set the levelSystem reference
        levelSystem = (StatistiquesLevelSystem)target;

        // Subscribe to level and XP change events
        levelSystem.onBaseStatsChange.AddListener(OnStatsChanged);
    }

    private void OnDisable()
    {
        // Unsubscribe from level and XP change events when the editor is disabled
        if (levelSystem != null)
        {
            levelSystem.onBaseStatsChange.RemoveListener(OnStatsChanged);
        }
    }

    // Callback method to be triggered when level or XP changes
    private void OnStatsChanged()
    {
        // Mark the editor as dirty and request a repaint
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        // Draw default fields for serialized properties
        DrawDefaultInspector();

        // Show runtime level and XP information
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Level & XP Info", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            // Display current level and XP with sliders
            int newLevel = EditorGUILayout.IntSlider("Current Level", levelSystem.CurrentLevel, 0, 100); // Level range can be adjusted
            if (newLevel != levelSystem.CurrentLevel)
            {
                levelSystem.SetLevelServerRPC(newLevel);
            }

            int newXp = EditorGUILayout.IntSlider("Current XP", levelSystem.CurrentXp, 0, 10000); // XP range can be adjusted
            if (newXp != levelSystem.CurrentXp)
            {
                levelSystem.SetXpServerRPC(newXp);
            }

            // TODO: Add Base Stats as Readonly

            // Buttons to trigger server RPCs
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            // Add parameter fields for each action
            addLevel = EditorGUILayout.IntField("Add Level", addLevel);
            addXp = EditorGUILayout.IntField("Add XP", addXp);

            // Action buttons with parameter input
            if (GUILayout.Button("Add Level"))
            {
                levelSystem.AddLevelServerRPC(addLevel); // Add the level to the user-defined value
            }

            if (GUILayout.Button("Add XP"))
            {
                levelSystem.AddXpServerRPC(addXp); // Add the XP to the user-defined value
            }
        }
    }
}
#endif
