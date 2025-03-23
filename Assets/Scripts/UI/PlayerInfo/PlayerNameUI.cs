using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
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
            var playerSettings = playerGo.GetComponent<PlayerSettings>();

            if (playerSettings != null)
            {
                UpdatePlayerName(playerSettings.GetPlayerName());

                playerSettings.OnPlayerNameChanged += UpdatePlayerName;
            }
        }
    }

    private void UpdatePlayerName(string newName)
    {
        if (playerNameText != null)
        {
            playerNameText.text = newName;
        }
    }

}
