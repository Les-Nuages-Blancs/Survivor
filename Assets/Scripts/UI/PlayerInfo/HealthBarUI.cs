using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private HealthSystem healthSystem;
    [SerializeField] private bool forceOwnerInfo = false;

    private void Start()
    {
        if (forceOwnerInfo)
        {
            foreach (Player player in Player.playerList)
            {
                SetupOwner(player.OwnerClientId);
            }
            Player.onPlayerAdded += SetupOwner;
        }
    }

    private void OnDestroy()
    {
        if (forceOwnerInfo)
        {
            Player.onPlayerAdded -= SetupOwner;
        }

        if (healthSystem != null)
        {
            healthSystem.onHealthChange.RemoveListener(UpdateHealthUI);
        }
    }

    private void SetupOwner(ulong clientId)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId) return;

        Setup(clientId);
    }

    public void Setup(ulong clientId)
    {
        Player player = Player.GetPlayerByClientId(clientId);

        if (player != null)
        {

            GameObject playerGo = player.gameObject;

            healthSystem = playerGo.GetComponent<HealthSystem>();

            if (healthSystem != null)
            {
                // Utiliser l'événement public onHealthChange
                healthSystem.onHealthChange.AddListener(UpdateHealthUI);

                // Mettre à jour la barre de vie avec les valeurs actuelles
                healthSlider.maxValue = healthSystem.MaxHealth;
                healthSlider.value = healthSystem.CurrentHealth;
            }
        }
        else
        {
            Debug.Log("player " + clientId + " not found");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null && healthSystem != null)
        {
            healthSlider.value = healthSystem.CurrentHealth;
        }
    }
}
