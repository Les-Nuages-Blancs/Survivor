using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Camera cameraToLookAt = null;

    private void Start()
    {
        if (cameraToLookAt) return;
        cameraToLookAt = Camera.main;
    }

    private void Update()
    {
        // transform.LookAt(transform.position + cameraToLookAt.transform.rotation * Vector3.back, cameraToLookAt.transform.rotation * Vector3.up);
        var lookRotation = cameraToLookAt.transform.rotation;
        transform.rotation = lookRotation;
    }
}
