using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Gate : MonoBehaviour
{
    [SerializeField] private float gateScale = 1.0f;
    [SerializeField] private List<GameObject> gatePlanes = new List<GameObject>();
    [SerializeField] private List<NavMeshObstacle> gateObstacle = new List<NavMeshObstacle>();

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

        foreach (NavMeshObstacle navObs in gateObstacle)
        {
            Vector3 currentScale = navObs.size;

            currentScale.z = 10 * gateScale;

            navObs.size = currentScale;

        }
    }
}
