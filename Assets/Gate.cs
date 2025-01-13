using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float gateScale = 1.0f;
    [SerializeField] private List<GameObject> gatePlanes = new List<GameObject>();

    private void OnValidate()
    {
        UpdateGateScale();
    }

    private void Start()
    {
        UpdateGateScale();
    }

    private void UpdateGateScale()
    {
        foreach (GameObject go in gatePlanes)
        {
            Vector3 currentScale = go.transform.localScale;

            // Set only the Z scale to a new value (e.g., 5f)
            currentScale.z = gateScale;

            // Apply the updated scale back to the GameObject
            go.transform.localScale = currentScale;
        }
    }
}
