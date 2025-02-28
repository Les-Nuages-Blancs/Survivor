using UnityEngine;
using System.Collections.Generic;

public class LineRendererUpdater : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private List<ElectricPoleLineHelper> poleHelpers = new List<ElectricPoleLineHelper>();
    [SerializeField] private List<ElectricLineHelperModifier> helperModifier = new List<ElectricLineHelperModifier>(); 
    [SerializeField] private int index = 0; // Base index for all connections
    [SerializeField] private float controlPointHeightOffset = 10f; // Public value to lower the Y position of the control points
    [SerializeField] private int bezierResolution = 20; // Number of Bezier points per segment
    [SerializeField, Range(0, 1)] private float ControlPointPositionPercentage = 0.5f;

    [SerializeField] private Vector3 windDirection = new Vector3(1, 0, 0); // Direction of wind (e.g., along X axis)
    [SerializeField] private float windStrength = 0f; // Strength of wind, controls how much sway the control points have

    [SerializeField] private bool autoUpdate = false;

    public float WindStrength
    {
        get => windStrength;
        set
        {
            if (windStrength != value)
            {
                windStrength = value;
                UpdateLine();
            }
        }
    }

    private void OnValidate()
    {
        // Automatically update the line if autoUpdate is enabled
        if (autoUpdate && lineRenderer != null)
        {
            UpdateLine();
        }
    }

    private void Reset()
    {
        // Ensure indexOffsets is initialized with the same size as poleHelpers
        helperModifier = new List<ElectricLineHelperModifier>(new ElectricLineHelperModifier[poleHelpers.Count]);
    }

    public void UpdateLine()
    {
        if (lineRenderer == null || poleHelpers.Count == 0)
        {
            Debug.LogWarning("LineRenderer or poleHelpers list is missing!");
            return;
        }

        // Ensure indexOffsets matches the number of poleHelpers
        if (helperModifier.Count != poleHelpers.Count)
        {
            Debug.LogWarning("IndexOffsets list size does not match PoleHelpers list size! Resetting...");
            Reset();
        }

        // Calculate total number of Bezier points
        int totalPoints = (poleHelpers.Count - 1) * bezierResolution;
        lineRenderer.positionCount = totalPoints; // Set the correct position count for the line

        Vector3 parentPos = lineRenderer.transform.position; // Get LineRenderer's position

        int pointIndex = 0; // Index for setting positions in the LineRenderer

        for (int i = 0; i < poleHelpers.Count - 1; i++)
        {
            int adjustedIndexStart = index + helperModifier[i].indexOffset; // Apply offset per helper
            int adjustedIndexEnd = index + helperModifier[i + 1].indexOffset;

            Transform startTransform = poleHelpers[i].GetLineTransformWithIndex(adjustedIndexStart);
            Transform endTransform = poleHelpers[i + 1].GetLineTransformWithIndex(adjustedIndexEnd);

            if (startTransform != null && endTransform != null)
            {
                // Calculate the middle point between the two pole transforms
                Vector3 middlePoint = startTransform.position + (endTransform.position - startTransform.position) * (helperModifier[i].overrideControlPointPositionPercentage ? helperModifier[i].controlPointPositionPercentage : ControlPointPositionPercentage);
                middlePoint.y -= (helperModifier[i].overrideControlPointHeightOffset ? helperModifier[i].controlPointHeightOffset : controlPointHeightOffset);

                // Apply wind effect to the middlePoint (control point)
                middlePoint += windDirection.normalized * windStrength;

                // Get Bezier curve points
                Vector3[] bezierPoints = GetQuadraticBezierPoints(startTransform.position, middlePoint, endTransform.position, helperModifier[i].overrideBezierResolution ? helperModifier[i].bezierResolution : bezierResolution);

                // Set the positions in the line renderer
                for (int j = 0; j < bezierPoints.Length; j++)
                {
                    lineRenderer.SetPosition(pointIndex, bezierPoints[j] - parentPos); // Convert to local space
                    pointIndex++;
                }
            }
        }
    }


    // Method to calculate quadratic bezier points
    private Vector3[] GetQuadraticBezierPoints(Vector3 p0, Vector3 p1, Vector3 p2, int resolution)
    {
        Vector3[] points = new Vector3[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            points[i] = CalculateQuadraticBezierPoint(t, p0, p1, p2);
        }
        return points;
    }

    // Calculate a single point on a quadratic bezier curve
    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return point;
    }
}
