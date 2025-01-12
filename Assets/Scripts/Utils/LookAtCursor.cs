using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class LookAtCursor : NetworkBehaviour
{
    private void RotateTowardsCursor()
    {
        if (!IsOwner) return;

        // Récupération de la position de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Détection de la position sur le sol (plan horizontal)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Vector3 lookAtPoint = hitInfo.point;
            lookAtPoint.y = transform.position.y; // Garde la rotation uniquement sur l'axe Y
            transform.LookAt(lookAtPoint);
        }
    }

    void Update()
    {
        RotateTowardsCursor();
    }
}
