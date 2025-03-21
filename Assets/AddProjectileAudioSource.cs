using UnityEngine;

public class AddProjectileAudioSource : MonoBehaviour
{
    public AudioSource projectileAudioSource; // La nouvelle AudioSource pour les projectiles

    private void Start()
    {
        AddIt();
    }

    private void OnEnable()
    {
        AddIt();
    }

    private void OnDestroy()
    {
        // Remove the AudioSource when this object is destroyed
        if (projectileAudioSource != null)
        {
            AudioManager.Instance.RemoveProjectileAudioSource(projectileAudioSource);
            Debug.Log("Projectile AudioSource removed.");
        }
    }

    private void AddIt()
    {
        if (projectileAudioSource != null)
        {
            AudioManager.Instance.AddProjectileAudioSource(projectileAudioSource);
            Debug.Log("Projectile AudioSource added.");
        }
        else
        {
            Debug.LogWarning("No Projectile AudioSource assigned.");
        }
    }
}
