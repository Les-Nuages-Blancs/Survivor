using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElectricLineHelperModifier
{
    [SerializeField] public int indexOffset = 0;

    [SerializeField] public bool overrideControlPointHeightOffset = false;
    [SerializeField] public float controlPointHeightOffset = 10.0f;

    [SerializeField] public bool overrideBezierResolution = false;
    [SerializeField] public int bezierResolution = 20;

    [SerializeField] public bool overrideControlPointPositionPercentage = false;
    [SerializeField, Range(0, 1)] public float controlPointPositionPercentage = 0.5f;
}