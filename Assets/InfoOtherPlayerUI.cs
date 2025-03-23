using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class InfoOtherPlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject playerInfoPrefab;

    private void Start()
    {
        foreach (Player player in Player.playerList)
        {
            ulong playerId = player.OwnerClientId;

            OnClientConnected(playerId);
        }
        Player.onPlayerAdded += OnClientConnected;
    }

    private void OnDestroy()
    {
        Player.onPlayerAdded -= OnClientConnected;
    }

    private void OnClientConnected(ulong newClientId)
    {
        if (NetworkManager.Singleton.LocalClientId == newClientId) return;

        GameObject newPlayerInfo = Instantiate(playerInfoPrefab, transform);

        SpecificPlayerInfoUI infoUI = newPlayerInfo.GetComponent<SpecificPlayerInfoUI>();
        if (infoUI != null)
        {
            infoUI.Setup(newClientId); 
        }
    }
}