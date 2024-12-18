using UnityEngine;

public class CameraFollowSystem : MonoBehaviour
{
    [SerializeField] public Transform target; // Référence au joueur
    [SerializeField] private Vector3 offset = new Vector3(0, 70, -30); // Position relative par rapport au joueur
    [SerializeField] private float smoothSpeed = 5f; // Vitesse de lissage du mouvement

    private void LateUpdate()
    {
        if (target)
        {
            // Calcul de la position cible de la caméra
            Vector3 targetPosition = target.position + offset;

            // Interpolation pour un mouvement fluide (facultatif)
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}