using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;

public class InfoOtherPlayerUI : NetworkBehaviour
{
    [SerializeField] private GameObject playerInfoPrefab;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }

    public override void OnNetworkSpawn()
    {
        foreach (ulong key in NetworkManager.Singleton.ConnectedClients.Keys)
        {
            OnClientConnected(key);
        }
    }

    private void OnClientConnected(ulong newClientId)
    {
        if (IsLocalPlayer(newClientId)) return;

        GameObject newPlayerInfo = Instantiate(playerInfoPrefab, transform);

        SpecificPlayerInfoUI infoUI = newPlayerInfo.GetComponent<SpecificPlayerInfoUI>();
        if (infoUI != null)
        {
            infoUI.Setup(newClientId); 
        }
    }

    private bool IsLocalPlayer(ulong clientId)
    {
        return NetworkManager.Singleton.LocalClientId == clientId;
    }
}