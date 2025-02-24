using UnityEngine;
using System.Collections.Generic;

public class ElectricPoleLineHelper : MonoBehaviour
{
    [SerializeField] private List<Transform> polePoints = new List<Transform>();
    [SerializeField] private int offsetBy = 0;

    public Transform GetLineTransformWithIndex(int index)
    {
        if (polePoints.Count == 0)
        {
            Debug.LogWarning("Pole points list is empty!");
            return null;
        }

        int loopedIndex = (index + offsetBy) % polePoints.Count; // Ensures it loops
        return polePoints[loopedIndex];
    }
}
