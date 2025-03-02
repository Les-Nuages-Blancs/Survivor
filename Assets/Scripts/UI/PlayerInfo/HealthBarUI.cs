using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private HealthSystem healthSystem;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;

        if (healthSystem != null)
        {
            healthSystem.onHealthChange.RemoveListener(UpdateHealthUI);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsLocalPlayer(clientId)) return;

        healthSystem = FindObjectOfType<HealthSystem>();

        if (healthSystem != null)
        {
            // Utiliser l'événement public onHealthChange
            healthSystem.onHealthChange.AddListener(UpdateHealthUI);

            // Mettre à jour la barre de vie avec les valeurs actuelles
            healthSlider.maxValue = healthSystem.MaxHealth;
            healthSlider.value = healthSystem.CurrentHealth;
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null && healthSystem != null)
        {
            healthSlider.value = healthSystem.CurrentHealth;
        }
    }

    private bool IsLocalPlayer(ulong clientId)
    {
        return NetworkManager.Singleton.LocalClientId == clientId;
    }
}
