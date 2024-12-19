using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float speed = 10f; // Vitesse du projectile

    private bool isDespawning = false;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Déplacer vers l'avant
    }

    public void TryKillProjectile()
    {
        if (!NetworkObject.IsSpawned) return;
        DestroyServerRPC();
    }

    [ServerRpc]
    public void DestroyServerRPC()
    {
        if (isDespawning) return;
        isDespawning = true;

        NetworkObject.Despawn();
        Destroy(gameObject);
    }
}