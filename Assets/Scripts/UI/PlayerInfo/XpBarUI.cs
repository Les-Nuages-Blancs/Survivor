using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class XpBarUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    private StatistiquesLevelSystem statLevelSystem;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;

        if (statLevelSystem != null)
        {
            statLevelSystem.onCurrentXpChange.RemoveListener(UpdateXpUI);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsLocalPlayer(clientId)) return;

        statLevelSystem = FindObjectOfType<StatistiquesLevelSystem>();

        if (statLevelSystem != null)
        {
            // Utiliser l'événement public onCurrentXpChange
            statLevelSystem.onCurrentXpChange.AddListener(UpdateXpUI);

            // Mettre à jour la barre d'XP avec les valeurs actuelles
            xpSlider.maxValue = statLevelSystem.BaseStatistiques.RequiredXpForNextLevel;
            xpSlider.value = statLevelSystem.CurrentXp;
        }
    }

    private void UpdateXpUI()
    {
        if (xpSlider != null && statLevelSystem != null)
        {
            xpSlider.maxValue = statLevelSystem.EntityLevelStatistiques.GetXpRequiredForNextLevel(statLevelSystem.CurrentLevel);
            xpSlider.value = statLevelSystem.CurrentXp;
        }
    }

    private bool IsLocalPlayer(ulong clientId)
    {
        return NetworkManager.Singleton.LocalClientId == clientId;
    }
}