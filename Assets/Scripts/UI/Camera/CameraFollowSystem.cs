using System;
using UnityEngine;

public class CameraFollowSystem : MonoBehaviour
{
    [SerializeField] public Transform target; // Référence au joueur
    [SerializeField] private Vector3 offset = new Vector3(0, 70, -30); // Position relative par rapport au joueur // Unused
    [SerializeField] private float smoothSpeed = 5f; // Vitesse de lissage du mouvement
    [SerializeField] private float horizontalAngle = -90; // Horizontal angle from the player
    [SerializeField] private float verticalAngle = 56; // Vertical angle from the player
    [SerializeField] private float distance = 25; // Distance from the player
    private float initialDistance;

    private void Start()
    {
        initialDistance = distance;
    }

    private void LateUpdate()
    {
        if (target)
        {
            // Calcul de la position cible de la caméra
            distance = initialDistance;
            var x = (float) Math.Cos(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
            var y = (float) Math.Sin(verticalAngle * Mathf.Deg2Rad);
            var z = (float) Math.Sin(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
            var direction = new Vector3(x, y, z).normalized;
            
            Vector3 targetPosition = target.position + direction * distance + 1.0f * Vector3.up;

            // Raycast pour detecter si un objet de l'environement bloque la vue du joueur
            var ray = new Ray(targetPosition + direction * 2.0f, -direction);
            Physics.Raycast(ray, out var hit, distance + 1.0f);
            while (hit.collider is not null && hit.collider.gameObject.layer == LayerMask.GetMask("Environment") && distance > 0.05f)
            {
                distance -= 0.01f;
                targetPosition = target.position + direction * distance + 1.0f * Vector3.up;

                ray = new Ray(targetPosition + direction * 2.0f, -direction);
                Physics.Raycast(ray, out hit, distance + 1.0f);
            }
                
            // Interpolation pour un mouvement fluide (facultatif)
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}