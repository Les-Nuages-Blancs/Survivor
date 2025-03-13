using UnityEngine;

public class MagnetObject : MonoBehaviour
{
    [Range(0f, 1f)] public float magnetMultiplier = 1f; // Reduction factor for range
    [SerializeField] private float speedIncreaseRate = 1f; // How fast the speed increases over time

    private bool isBeingMagnetized = false;
    private Transform target;
    private float baseAttractionSpeed;
    private float currentAttractionSpeed;
    private float timeBeingMagnetized = 0f;

    public bool IsBeingMagnetized => isBeingMagnetized;
    public float MagnetMultiplier => magnetMultiplier;

    public void StartMagnet(MagnetPlayer player, float speed)
    {
        if (isBeingMagnetized) return;

        isBeingMagnetized = true;
        target = player.transform;
        baseAttractionSpeed = speed;
        currentAttractionSpeed = speed;
        timeBeingMagnetized = 0f; // Reset time counter
    }

    private void Update()
    {
        if (isBeingMagnetized && target != null)
        {
            // Increase time being magnetized
            timeBeingMagnetized += Time.deltaTime;

            // Gradually increase attraction speed
            currentAttractionSpeed = baseAttractionSpeed + (speedIncreaseRate * timeBeingMagnetized);

            // Move object towards target
            transform.position = Vector3.MoveTowards(transform.position, target.position, currentAttractionSpeed * Time.deltaTime);
        }
    }
}
