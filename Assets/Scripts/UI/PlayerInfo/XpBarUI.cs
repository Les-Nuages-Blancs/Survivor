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

        if (statLevelSystem != null)
        {
            statLevelSystem.onCurrentXpChange.RemoveListener(UpdateXpUI);
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

        GameObject playerGo = player.gameObject;

        statLevelSystem = playerGo.GetComponent<StatistiquesLevelSystem>();

        if (statLevelSystem != null)
        {
            // Utiliser l'�v�nement public onCurrentXpChange
            statLevelSystem.onCurrentXpChange.AddListener(UpdateXpUI);

            // Mettre � jour la barre d'XP avec les valeurs actuelles
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
}