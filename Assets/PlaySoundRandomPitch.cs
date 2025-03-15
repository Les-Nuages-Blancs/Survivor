using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundRandomPitch : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.2f;
    [SerializeField] private bool playOnStart = false;

    private void Start()
    {
        if (playOnStart)
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned!", this);
            return;
        }

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
}
