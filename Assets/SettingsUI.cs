using UnityEngine;

using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Dropdown qualityDropdown;
    public Toggle vSyncToggle;
    public Slider frameRateSlider;
    public Text frameRateText;
    public Toggle unlimitedFrameRateToggle;

    private void Start()
    {
        // Remplir le dropdown avec les niveaux de qualité disponibles
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        // Initialiser les autres options UI avec les valeurs actuelles
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vSyncToggle.onValueChanged.AddListener(ToggleVSync);
        frameRateSlider.value = Mathf.Clamp(Application.targetFrameRate, 30, 144);
        frameRateSlider.onValueChanged.AddListener(SetFrameRate);  // Lier le slider à la fonction SetFrameRate
        unlimitedFrameRateToggle.isOn = Application.targetFrameRate == -1;
        unlimitedFrameRateToggle.onValueChanged.AddListener(ToggleFrameRateLimit);  // Lier le toggle illimité à la fonction ToggleFrameRateLimit
        UpdateFrameRateText((int)frameRateSlider.value);
        ToggleFrameRateLimit(unlimitedFrameRateToggle.isOn);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
        Debug.Log("Quality set to: " + QualitySettings.names[qualityIndex]);
    }

    public void ToggleVSync(bool isEnabled)
    {
        QualitySettings.vSyncCount = isEnabled ? 1 : 0;
        Debug.Log("VSync " + (isEnabled ? "enabled" : "disabled"));
    }

    public void SetFrameRate(float frameRate)
    {
        if (!unlimitedFrameRateToggle.isOn)
        {
            Application.targetFrameRate = (int)frameRate;
            UpdateFrameRateText((int)frameRate);
            Debug.Log("Frame rate set to: " + Application.targetFrameRate);
        }
    }

    public void ToggleFrameRateLimit(bool isUnlimited)
    {
        if (isUnlimited)
        {
            Application.targetFrameRate = -1;
            frameRateSlider.interactable = false;
            frameRateText.text = "Unlimited";
            Debug.Log("Frame rate set to Unlimited");
        }
        else
        {
            frameRateSlider.interactable = true;
            SetFrameRate(frameRateSlider.value);
        }
    }

    private void UpdateFrameRateText(int value)
    {
        if (frameRateText != null)
        {
            frameRateText.text = value + " FPS";
        }
    }
}
