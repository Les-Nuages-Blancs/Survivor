using Unity.Netcode;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Vitesse du projectile
    public bool isReal; //Will the projectil deal damage on collision with ennemy ?

    
    void FixedUpdate()
    {
        float fac = isReal ? 0.3f : 0.1f; //tmp debug to see if isreal is correctly set up
        transform.Translate(fac * Vector3.forward * speed * Time.deltaTime); // D�placer vers l'avant
    }

    //collider venant du dmg system
}