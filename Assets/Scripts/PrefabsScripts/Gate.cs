using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class Gate : NetworkBehaviour
{
    [SerializeField] private float gateScale = 1.0f;
    [SerializeField] private List<GameObject> gatePlanes = new List<GameObject>();
    [SerializeField] private List<NavMeshObstacle> gateObstacle = new List<NavMeshObstacle>();
    [SerializeField] private GameObject unlockZoneRight;
    [SerializeField] private GameObject unlockZoneLeft;
    [SerializeField] private bool unlockZoneLeftVisible = true;
    [SerializeField] private bool unlockZoneRightVisible = true;

    private bool canBeUnlocked = false;

    public void UnlockGate()
    {
        canBeUnlocked = true;
        UpdateUnlockZoneVisibility();
    }

    private void OnValidate()
    {
        UpdateGateScale();
        UpdateUnlockZoneVisibility();
    }

    private void Start()
    {
        unlockZoneLeft.GetComponent<TriggerStayEvent>().gate = this;
        unlockZoneRight.GetComponent<TriggerStayEvent>().gate = this;
        UpdateGateScale();
        UpdateUnlockZoneVisibility();
    }

    private void UpdateUnlockZoneVisibility()
    {
        unlockZoneLeft.SetActive(unlockZoneLeftVisible && canBeUnlocked);
        unlockZoneRight.SetActive(unlockZoneRightVisible && canBeUnlocked);
    }

    private void UpdateUnlockZoneScale(GameObject go)
    {
        Vector3 currentScale = go.transform.localScale;
        Vector3 currentPosition = go.transform.localPosition;

        // Set only the Z scale to a new value (e.g., 5f)
        currentScale.z = gateScale;
        currentPosition.z = gateScale * 5;

        // Apply the updated scale back to the GameObject
        go.transform.localScale = currentScale;
        go.transform.localPosition = currentPosition;
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

        UpdateUnlockZoneScale(unlockZoneLeft);
        UpdateUnlockZoneScale(unlockZoneRight);

        foreach (NavMeshObstacle navObs in gateObstacle)
        {
            Vector3 currentScale = navObs.size;

            currentScale.z = 10 * gateScale;

            navObs.size = currentScale;

        }
    }
}
