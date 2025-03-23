using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LvlUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText; 
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
            statLevelSystem.onCurrentLevelChange.RemoveListener(UpdateLevelText);
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

            statLevelSystem = playerGo.GetComponent<StatistiquesLevelSystem>();

            if (statLevelSystem != null)
            {
                statLevelSystem.onCurrentLevelChange.AddListener(UpdateLevelText);

                UpdateLevelText();
            }
        }
    }

    private void UpdateLevelText()
    {
        if (levelText != null && statLevelSystem != null)
        {
            levelText.text = $"{statLevelSystem.CurrentLevel}";
        }
    }
}
