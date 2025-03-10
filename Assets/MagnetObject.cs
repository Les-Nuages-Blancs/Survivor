using UnityEngine;

public class MagnetObject : MonoBehaviour
{
    [Range(0f, 1f)] public float magnetMultiplier = 1f; // Facteur de réduction de la range
    private bool isBeingMagnetized = false;
    private Transform target;
    private float attractionSpeed;

    public bool IsBeingMagnetized => isBeingMagnetized;
    public float MagnetMultiplier => magnetMultiplier;

    public void StartMagnet(MagnetPlayer player, float speed)
    {
        if (isBeingMagnetized) return;

        isBeingMagnetized = true;
        target = player.transform;
        attractionSpeed = speed;
    }

    private void Update()
    {
        if (isBeingMagnetized && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, attractionSpeed * Time.deltaTime);
        }
    }
}
