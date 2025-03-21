using UnityEngine;

public class AddMusicAudioSource : MonoBehaviour
{
    public AudioSource musicAudioSource; // La nouvelle AudioSource à ajouter

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
        if (musicAudioSource != null)
        {
            AudioManager.Instance.RemoveMusicAudioSource(musicAudioSource);
        }
    }

    private void AddIt()
    {
        if (musicAudioSource != null)
        {
            AudioManager.Instance.AddMusicAudioSource(musicAudioSource);
        }
        else
        {
            Debug.LogWarning("No Music AudioSource assigned.");
        }
    }
}
