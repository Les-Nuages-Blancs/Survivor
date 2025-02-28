using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindOnLine : MonoBehaviour
{
    [SerializeField] private LineRendererUpdater lineUpdater;
    [SerializeField] private float minWindStrength = 0.5f;
    [SerializeField] private float maxWindStrength = 2f;
    [SerializeField] private float noiseScale = 1.0f; // Controls how fast the wind changes

    private float noiseOffset;

    private void Start()
    {
        noiseOffset = Random.Range(0f, 1000f); // Avoids all instances having the same noise pattern
    }

    private void Update()
    {
        float noiseValue = Mathf.PerlinNoise(Time.time * noiseScale, noiseOffset);
        lineUpdater.WindStrength = Mathf.Lerp(minWindStrength, maxWindStrength, noiseValue);
    }
}
