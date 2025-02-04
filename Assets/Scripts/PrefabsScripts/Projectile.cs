using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float speed = 10f; // Vitesse du projectile

    void Update()
    {
        if(!IsOwner) return;
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // D�placer vers l'avant
    }
}