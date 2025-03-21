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
            Debug.Log("Music AudioSource removed.");
        }
    }

    private void AddIt()
    {
        if (musicAudioSource != null)
        {
            AudioManager.Instance.AddMusicAudioSource(musicAudioSource);
            Debug.Log("Music AudioSource added.");
        }
        else
        {
            Debug.LogWarning("No Music AudioSource assigned.");
        }
    }
}
