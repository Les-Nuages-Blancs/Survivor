//using UnityEngine;
//using UnityEngine.UI;
//using Unity.Netcode;

//public class HealthBarUI : MonoBehaviour
//{
//    [SerializeField] private Slider healthSlider; // Référence au Slider de l'UI
//    private HealthSystem healthSystem;

//    private void Start()
//    {
//        // Trouver automatiquement le joueur local avec Netcode
//        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
//    }

//    private void OnDestroy()
//    {
//        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;

//        if (healthSystem != null)
//        {
//            healthSystem.OnHealthChanged -= UpdateHealthUI;
//        }
//    }

//    private void OnClientConnected(ulong clientId)
//    {
//        if (!IsLocalPlayer(clientId)) return;

//        // Trouver le HealthSystem du joueur local
//        healthSystem = FindObjectOfType<HealthSystem>();

//        if (healthSystem != null)
//        {
//            // Abonnement aux changements de vie
//            healthSystem.currentHealth.OnValueChanged += OnHealthChanged;

//            // Initialiser la barre de vie
//            healthSlider.maxValue = healthSystem.MaxHealth;
//            healthSlider.value = healthSystem.CurrentHealth;
//        }
//    }

//    private void OnHealthChanged(float oldValue, float newValue)
//    {
//        if (healthSlider != null)
//        {
//            healthSlider.value = newValue;
//        }
//    }

//    private bool IsLocalPlayer(ulong clientId)
//    {
//        return NetworkManager.Singleton.LocalClientId == clientId;
//    }
//}

