using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Vector3 rotationSpeed; // Rotation speed in degrees per second for each axis (X, Y, Z)

    // Update is called once per frame
    void Update()
    {
        // Rotate the object based on the speed vector and deltaTime for frame-independent rotation
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
