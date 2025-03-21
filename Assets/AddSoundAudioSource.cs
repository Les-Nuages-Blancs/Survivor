using UnityEngine;

public class AddSoundAudioSource : MonoBehaviour
{
    public AudioSource soundAudioSource; // La nouvelle AudioSource à ajouter

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
        if (soundAudioSource != null)
        {
            AudioManager.Instance.RemoveSoundAudioSource(soundAudioSource);
        }
    }

    private void AddIt()
    {
        if (soundAudioSource != null)
        {
            AudioManager.Instance.AddSoundAudioSource(soundAudioSource);
        }
        else
        {
            Debug.LogWarning("No Sound AudioSource assigned.");
        }
    }
}
