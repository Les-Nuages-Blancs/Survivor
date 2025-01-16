#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnZoneLevelRangeUpgrader))]
public class SpawnZoneLevelRangeUpgraderDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Access the parent array and current element index
        SerializedProperty arrayProperty = property.serializedObject.FindProperty(property.propertyPath.Substring(0, property.propertyPath.LastIndexOf('.')));
        int index = DrawerHelper.GetArrayIndex(property);

        // Access the "numberOfLevel" property
        SerializedProperty numberOfLevelProperty = property.FindPropertyRelative("numberOfLevel");

        // Calculate the starting level based on previous elements
        int startLevel = 1;
        for (int i = 0; i < index; i++)
        {
            SerializedProperty previousElement = arrayProperty.GetArrayElementAtIndex(i);
            SerializedProperty previousNumberOfLevel = previousElement.FindPropertyRelative("numberOfLevel");
            startLevel += previousNumberOfLevel.intValue;
        }

        // Calculate the end level
        int numberOfLevel = numberOfLevelProperty.intValue;
        int endLevel = startLevel + numberOfLevel - 1;

        // Modify the label to display the dynamic range
        label.text = $"Level Range: {startLevel} - {endLevel}";

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
