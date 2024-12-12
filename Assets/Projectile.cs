using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f; // Vitesse du projectile
    public float maxLifeTime = 5f; // Temps avant destruction

    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject HitEffectPrefab;

    void Start()
    {
        Destroy(gameObject, maxLifeTime); // D�truire apr�s un certain temps
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // D�placer vers l'avant
    }

    void OnTriggerEnter(Collider other)
    {
        if (HitEffectPrefab != null)
        {
            Instantiate(HitEffectPrefab, other.transform.position, other.transform.rotation);
        }

        Destroy(gameObject);
    }
}