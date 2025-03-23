using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class XpBarUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    private StatistiquesLevelSystem statLevelSystem;
    [SerializeField] private bool forceOwnerInfo = false;

    private void Start()
    {
        if (forceOwnerInfo)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }
    }

    private void OnDestroy()
    {
        if (forceOwnerInfo)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }

        if (statLevelSystem != null)
        {
            statLevelSystem.onCurrentXpChange.RemoveListener(UpdateXpUI);
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (!IsLocalPlayer(clientId)) return;

        Setup(clientId);
    }

    public void Setup(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var networkClient))
        {
            GameObject playerGo = networkClient.PlayerObject.gameObject;

            statLevelSystem = playerGo.GetComponent<StatistiquesLevelSystem>();

            if (statLevelSystem != null)
            {
                // Utiliser l'événement public onCurrentXpChange
                statLevelSystem.onCurrentXpChange.AddListener(UpdateXpUI);

                // Mettre à jour la barre d'XP avec les valeurs actuelles
                xpSlider.maxValue = statLevelSystem.BaseStatistiques.RequiredXpForNextLevel;
                xpSlider.value = statLevelSystem.CurrentXp;
            }
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