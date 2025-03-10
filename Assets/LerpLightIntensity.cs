using UnityEngine;

public class LerpLightIntensity : MonoBehaviour
{
    [SerializeField] private Light lightToDim; // Reference to the Light component
    [SerializeField] private float dimDuration = 2f; // Time it takes to dim the light to 0

    private float originalIntensity; // Store the original intensity
    private float targetIntensity = 0f; // Target intensity (0)
    private float lerpTime = 0f; // Track the lerping progress

    private bool isLerping = false; // Flag to track if lerping is in progress

    private void Update()
    {
        if (isLerping && lightToDim != null)
        {
            // Increment lerpTime based on elapsed time
            lerpTime += Time.deltaTime / dimDuration;

            // Lerp the light intensity from current to 0
            lightToDim.intensity = Mathf.Lerp(originalIntensity, targetIntensity, lerpTime);

            // If the intensity has reached 0, stop lerping
            if (lightToDim.intensity <= 0.01f)
            {
                lightToDim.intensity = 0f; // Ensure it's exactly 0
                isLerping = false; // Stop lerping
            }
        }
    }

    // Public method to start the lerp process
    public void StartLerp()
    {
        if (lightToDim != null)
        {
            originalIntensity = lightToDim.intensity; // Store the current intensity when lerping starts
            lerpTime = 0f; // Reset the lerp time
            isLerping = true; // Start the lerping process
        }
    }
}
