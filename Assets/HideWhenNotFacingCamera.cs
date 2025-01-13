using UnityEngine;

public class HideWhenNotFacingCamera : MonoBehaviour
{
    public Camera targetCamera; // La caméra à suivre (souvent la Main Camera)
    public Canvas canvas;       // Le canvas contenant le texte
    public bool reverseCondition = false;

    private void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        if (canvas == null)
            canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (canvas == null || targetCamera == null) return;

        // Calculer le vecteur avant du canvas
        Vector3 canvasForward;
        if (reverseCondition)
        {
            canvasForward = canvas.transform.forward;
        }
        else
        {
            canvasForward = -canvas.transform.forward;
        }

        // Calculer le vecteur direction vers la caméra
        Vector3 directionToCamera = targetCamera.transform.position - canvas.transform.position;

        // Calculer l'angle entre le vecteur avant et la direction vers la caméra
        float angle = Vector3.Angle(canvasForward, directionToCamera);

        // Afficher ou cacher le canvas en fonction de l'angle
        canvas.enabled = angle < 90f;
    }
}
