#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineRendererUpdater)), CanEditMultipleObjects]
public class LineRendererUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        // Loop through all selected objects and create a button that works for each
        if (GUILayout.Button("Update Line"))
        {
            foreach (var targetObject in targets)
            {
                LineRendererUpdater script = (LineRendererUpdater)targetObject;
                script.UpdateLine();
            }
        }
    }
}
#endif
