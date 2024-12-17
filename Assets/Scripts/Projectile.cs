using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float speed = 10f; // Vitesse du projectile

    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject HitEffectPrefab;

    private bool isDespawning = false;

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Déplacer vers l'avant
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (isDespawning) return;
        isDespawning = true;


        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            HealthSystem healthSystem = forwarder.ForwardedGameObject.GetComponent<HealthSystem>();
            if (healthSystem)
            {
                healthSystem.TakeDamage(damage);
            }
        }

        if (HitEffectPrefab != null)
        {
            SpawnParticleServerRPC();
        }

        DestroyServerRPC();
    }

    [ServerRpc]
    private void SpawnParticleServerRPC()
    {
        GameObject go = Instantiate(HitEffectPrefab, transform.position, transform.rotation);
        NetworkObject networkObject = go.GetComponent<NetworkObject>();
        networkObject.Spawn();
    }

    [ServerRpc]
    private void DestroyServerRPC()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}