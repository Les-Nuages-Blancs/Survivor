#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HealthSystem))]
public class HealthSystemEditor : Editor
{
    // Variables to hold parameter values for the buttons
    private float addHpAmount = 10f;
    private float addHpPercent = 0.1f;
    private float damageAmount = 10f;

    // Store references to HealthSystem to listen for changes
    private HealthSystem healthSystem;

    private void OnEnable()
    {
        // Set the healthSystem reference
        healthSystem = (HealthSystem)target;

        // Subscribe to health change events
        healthSystem.onHealthChange.AddListener(OnHealthChanged);
        healthSystem.onMaxHealthChange.AddListener(OnMaxHealthChanged);
    }

    private void OnDisable()
    {
        // Unsubscribe from health change events when the editor is disabled
        if (healthSystem != null)
        {
            healthSystem.onHealthChange.RemoveListener(OnHealthChanged);
            healthSystem.onMaxHealthChange.RemoveListener(OnMaxHealthChanged);
        }
    }

    // Callback method to be triggered when health changes
    private void OnHealthChanged()
    {
        // Mark the editor as dirty and request a repaint
        Repaint();
    }

    // Callback method to be triggered when max health changes
    private void OnMaxHealthChanged()
    {
        // Mark the editor as dirty and request a repaint
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        // Draw default fields for serialized properties
        DrawDefaultInspector();

        // Show runtime health information
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Health Info", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            // Display current health with scroller to modify it
            float newHealth = EditorGUILayout.Slider("Current Health", healthSystem.CurrentHealth, 0, healthSystem.MaxHealth);
            if (Application.isPlaying && Mathf.Abs(newHealth - healthSystem.CurrentHealth) > Mathf.Epsilon)
            {
                if (newHealth > healthSystem.CurrentHealth)
                {
                    healthSystem.AddHpServerRPC(newHealth - healthSystem.CurrentHealth);
                }
                else
                {
                    healthSystem.TakeDamageServerRPC(healthSystem.CurrentHealth - newHealth);
                }
            }

            // Display max health (readonly)
            EditorGUILayout.FloatField("Max Health", healthSystem.MaxHealth);

            // Buttons to trigger server RPCs
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            // Add parameter fields for each action
            addHpAmount = EditorGUILayout.FloatField("Add HP Amount", addHpAmount);
            addHpPercent = EditorGUILayout.FloatField("Add HP By Percent of Max HP", addHpPercent);
            damageAmount = EditorGUILayout.FloatField("Damage Amount", damageAmount);

            // Action buttons with parameter input
            if (GUILayout.Button("Add HP"))
            {
                healthSystem.AddHpServerRPC(addHpAmount); // Use the user-defined value
            }

            if (GUILayout.Button("Add HP By Percent of Max HP"))
            {
                healthSystem.AddHpByPercentOfMaxHpServerRPC(addHpPercent); // Use the user-defined percentage
            }

            if (GUILayout.Button("Regen All HP"))
            {
                healthSystem.RegenAllHpServerRPC(); // No parameters needed
            }

            if (GUILayout.Button("Take Damage"))
            {
                healthSystem.TakeDamageServerRPC(damageAmount); // Use the user-defined damage value
            }
        }
    }
}
#endif
