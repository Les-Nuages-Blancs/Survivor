#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawerHelper : MonoBehaviour
{
    // Helper to get the index of the current element in the array
    static public int GetArrayIndex(SerializedProperty property)
    {
        string path = property.propertyPath;
        string indexString = path.Substring(path.LastIndexOf('[') + 1, path.LastIndexOf(']') - path.LastIndexOf('[') - 1);
        if (int.TryParse(indexString, out int index))
        {
            return index;
        }
        return 0; // Default to 0 if parsing fails
    }
}
#endif