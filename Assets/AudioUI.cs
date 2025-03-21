using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle soundToggle;           // Toggle pour activer/désactiver le son
    public Toggle musicToggle;           // Toggle pour activer/désactiver la musique
    public Toggle projectileToggle;      // Toggle pour activer/désactiver le son des projectiles
    public Slider soundVolumeSlider;     // Slider pour le volume du son
    public Slider musicVolumeSlider;     // Slider pour le volume de la musique
    public Slider projectileVolumeSlider; // Slider pour le volume des projectiles
    public Text soundVolumeText;         // Texte affichant le volume du son
    public Text musicVolumeText;         // Texte affichant le volume de la musique
    public Text projectileVolumeText;    // Texte affichant le volume des projectiles

    private void Start()
    {
        // Initialiser les valeurs des options audio UI avec les paramètres actuels depuis PlayerPrefs
        soundToggle.isOn = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        musicToggle.isOn = PlayerPrefs.GetInt("MusicEnabled", 1) == 1;
        projectileToggle.isOn = PlayerPrefs.GetInt("ProjectileEnabled", 1) == 1; // Charger l'état des projectiles
        soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        projectileVolumeSlider.value = PlayerPrefs.GetFloat("ProjectileVolume", 1f); // Charger le volume des projectiles

        // Lier les UI elements aux fonctions correspondantes
        soundToggle.onValueChanged.AddListener(ToggleSound);
        musicToggle.onValueChanged.AddListener(ToggleMusic);
        projectileToggle.onValueChanged.AddListener(ToggleProjectile);
        soundVolumeSlider.onValueChanged.AddListener(SetSoundVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        projectileVolumeSlider.onValueChanged.AddListener(SetProjectileVolume);

        // Mettre à jour les textes de volume
        UpdateSoundVolumeText();
        UpdateMusicVolumeText();
        UpdateProjectileVolumeText();
    }

    public void ToggleSound(bool isEnabled)
    {
        // Appeler la méthode du AudioManager (singleton) pour activer/désactiver le son
        AudioManager.Instance.SetSoundEnabled(isEnabled);
        Debug.Log("Sound " + (isEnabled ? "enabled" : "disabled"));
    }

    public void ToggleMusic(bool isEnabled)
    {
        // Appeler la méthode du AudioManager (singleton) pour activer/désactiver la musique
        AudioManager.Instance.SetMusicEnabled(isEnabled);
        Debug.Log("Music " + (isEnabled ? "enabled" : "disabled"));
    }

    public void ToggleProjectile(bool isEnabled)
    {
        // Appeler la méthode du AudioManager (singleton) pour activer/désactiver le son des projectiles
        AudioManager.Instance.SetProjectileEnabled(isEnabled);
        Debug.Log("Projectile sound " + (isEnabled ? "enabled" : "disabled"));
    }

    public void SetSoundVolume(float volume)
    {
        // Appeler la méthode du AudioManager (singleton) pour ajuster le volume du son
        AudioManager.Instance.SetSoundVolume(volume);
        UpdateSoundVolumeText();
        Debug.Log("Sound volume set to: " + volume);
    }

    public void SetMusicVolume(float volume)
    {
        // Appeler la méthode du AudioManager (singleton) pour ajuster le volume de la musique
        AudioManager.Instance.SetMusicVolume(volume);
        UpdateMusicVolumeText();
        Debug.Log("Music volume set to: " + volume);
    }

    public void SetProjectileVolume(float volume)
    {
        // Appeler la méthode du AudioManager (singleton) pour ajuster le volume des projectiles
        AudioManager.Instance.SetProjectileVolume(volume);
        UpdateProjectileVolumeText();
        Debug.Log("Projectile volume set to: " + volume);
    }

    private void UpdateSoundVolumeText()
    {
        if (soundVolumeText != null)
        {
            soundVolumeText.text = (soundVolumeSlider.value * 100).ToString("F0") + "%";
        }
    }

    private void UpdateMusicVolumeText()
    {
        if (musicVolumeText != null)
        {
            musicVolumeText.text = (musicVolumeSlider.value * 100).ToString("F0") + "%";
        }
    }

    private void UpdateProjectileVolumeText()
    {
        if (projectileVolumeText != null)
        {
            projectileVolumeText.text = (projectileVolumeSlider.value * 100).ToString("F0") + "%";
        }
    }
}
