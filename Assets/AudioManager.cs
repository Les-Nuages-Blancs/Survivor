using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public List<AudioSource> soundAudioSources = new List<AudioSource>(); // Liste des AudioSource pour les effets sonores
    public List<AudioSource> musicAudioSources = new List<AudioSource>(); // Liste des AudioSource pour la musique
    public List<AudioSource> projectileAudioSources = new List<AudioSource>(); // Liste des AudioSource pour les projectiles

    private const string SOUND_PREF_KEY = "SoundEnabled";
    private const string MUSIC_PREF_KEY = "MusicEnabled";
    private const string SOUND_VOLUME_PREF_KEY = "SoundVolume";
    private const string MUSIC_VOLUME_PREF_KEY = "MusicVolume";
    private const string PROJECTILE_PREF_KEY = "ProjectileEnabled";  // Pr�f�rence pour activer/d�sactiver les projectiles
    private const string PROJECTILE_VOLUME_PREF_KEY = "ProjectileVolume";  // Pr�f�rence pour le volume des projectiles

    // Singleton instance
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Assurer qu'il n'y ait qu'une seule instance de AudioManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Garder l'AudioManager lors des sc�nes changeantes

        // Charger les pr�f�rences de l'utilisateur
        bool soundEnabled = PlayerPrefs.GetInt(SOUND_PREF_KEY, 1) == 1;
        bool musicEnabled = PlayerPrefs.GetInt(MUSIC_PREF_KEY, 1) == 1;
        bool projectileEnabled = PlayerPrefs.GetInt(PROJECTILE_PREF_KEY, 1) == 1; // Charger la pr�f�rence des projectiles
        float soundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_PREF_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF_KEY, 1f);
        float projectileVolume = PlayerPrefs.GetFloat(PROJECTILE_VOLUME_PREF_KEY, 1f); // Charger le volume des projectiles

        // Appliquer les pr�f�rences audio
        SetSoundEnabled(soundEnabled);
        SetMusicEnabled(musicEnabled);
        SetProjectileEnabled(projectileEnabled); // Appliquer la pr�f�rence pour les projectiles
        SetSoundVolume(soundVolume);
        SetMusicVolume(musicVolume);
        SetProjectileVolume(projectileVolume); // Appliquer le volume des projectiles
    }

    // Ajouter une AudioSource pour les effets sonores
    public void AddSoundAudioSource(AudioSource audioSource)
    {
        if (!soundAudioSources.Contains(audioSource))
        {
            soundAudioSources.Add(audioSource);

            // Synchroniser les r�glages de la nouvelle AudioSource avec les pr�f�rences
            audioSource.mute = PlayerPrefs.GetInt(SOUND_PREF_KEY, 1) == 0; // Activer/d�sactiver en fonction des pr�f�rences
            audioSource.volume = PlayerPrefs.GetFloat(SOUND_VOLUME_PREF_KEY, 1f); // Appliquer le volume
        }
    }

    // Supprimer une AudioSource des effets sonores
    public void RemoveSoundAudioSource(AudioSource audioSource)
    {
        if (soundAudioSources.Contains(audioSource))
        {
            soundAudioSources.Remove(audioSource);
        }
    }

    // Ajouter une AudioSource pour la musique
    public void AddMusicAudioSource(AudioSource audioSource)
    {
        if (!musicAudioSources.Contains(audioSource))
        {
            musicAudioSources.Add(audioSource);

            // Synchroniser les r�glages de la nouvelle AudioSource avec les pr�f�rences
            audioSource.mute = PlayerPrefs.GetInt(MUSIC_PREF_KEY, 1) == 0; // Activer/d�sactiver en fonction des pr�f�rences
            audioSource.volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF_KEY, 1f); // Appliquer le volume
        }
    }

    // Supprimer une AudioSource de la musique
    public void RemoveMusicAudioSource(AudioSource audioSource)
    {
        if (musicAudioSources.Contains(audioSource))
        {
            musicAudioSources.Remove(audioSource);
        }
    }

    // Ajouter une AudioSource pour les projectiles
    public void AddProjectileAudioSource(AudioSource audioSource)
    {
        if (!projectileAudioSources.Contains(audioSource))
        {
            projectileAudioSources.Add(audioSource);

            // Synchroniser les r�glages de la nouvelle AudioSource avec les pr�f�rences
            audioSource.mute = PlayerPrefs.GetInt(PROJECTILE_PREF_KEY, 1) == 0; // Activer/d�sactiver en fonction des pr�f�rences
            audioSource.volume = PlayerPrefs.GetFloat(PROJECTILE_VOLUME_PREF_KEY, 1f); // Appliquer le volume
        }
    }

    // Supprimer une AudioSource des projectiles
    public void RemoveProjectileAudioSource(AudioSource audioSource)
    {
        if (projectileAudioSources.Contains(audioSource))
        {
            projectileAudioSources.Remove(audioSource);
        }
    }

    // Fonction pour activer/d�sactiver les effets sonores (muter toutes les AudioSource des effets sonores)
    public void SetSoundEnabled(bool isEnabled)
    {
        foreach (var audioSource in soundAudioSources)
        {
            audioSource.mute = !isEnabled;
        }
        PlayerPrefs.SetInt(SOUND_PREF_KEY, isEnabled ? 1 : 0);
    }

    // Fonction pour activer/d�sactiver la musique (muter toutes les AudioSource de la musique)
    public void SetMusicEnabled(bool isEnabled)
    {
        foreach (var audioSource in musicAudioSources)
        {
            audioSource.mute = !isEnabled;
        }
        PlayerPrefs.SetInt(MUSIC_PREF_KEY, isEnabled ? 1 : 0);
    }

    // Fonction pour r�gler le volume des effets sonores
    public void SetSoundVolume(float volume)
    {
        foreach (var audioSource in soundAudioSources)
        {
            audioSource.volume = volume;
        }
        PlayerPrefs.SetFloat(SOUND_VOLUME_PREF_KEY, volume);
    }

    // Fonction pour r�gler le volume de la musique
    public void SetMusicVolume(float volume)
    {
        foreach (var audioSource in musicAudioSources)
        {
            audioSource.volume = volume;
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PREF_KEY, volume);
    }

    // Fonction pour r�gler le volume des audio sources des projectiles
    public void SetProjectileVolume(float volume)
    {
        foreach (var audioSource in projectileAudioSources)
        {
            audioSource.volume = volume;
        }
        PlayerPrefs.SetFloat(PROJECTILE_VOLUME_PREF_KEY, volume);
    }

    // Fonction pour activer/d�sactiver les audio sources des projectiles
    public void SetProjectileEnabled(bool isEnabled)
    {
        foreach (var audioSource in projectileAudioSources)
        {
            audioSource.mute = !isEnabled;
        }
        PlayerPrefs.SetInt(PROJECTILE_PREF_KEY, isEnabled ? 1 : 0);
    }
}
