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

        const float offset = 0;

        // R�cup�ration de la position de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0,transform.position.y+offset,0));
        float enter;

        // D�tection de la position sur un plan horizontal à la hauteur du joueur
        if (plane.Raycast(ray, out enter))
        {
            Vector3 lookAtPoint = ray.GetPoint(enter);
            //lookAtPoint.y = transform.position.y; // Garde la rotation uniquement sur l'axe Y
            //pas besoin de modifier la coordonné y on peut directement jouer dessus en modifiant l'offset
            transform.LookAt(lookAtPoint);
        }
    }

    void Update()
    {
        RotateTowardsCursor();
    }
}
