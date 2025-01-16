#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnZoneLevelAttribute))]
public class LevelSpawnZoneDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Access the current element index
        int index = DrawerHelper.GetArrayIndex(property);

        // Modify the label to display the targeted level
        if (index == 0)
        {
            label.text = $"Base Datas (Level {index})";
        }
        else
        {
            label.text = $"Level {index}";
        }

        // Draw the property with the modified label
        EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Return the default height for the property
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif