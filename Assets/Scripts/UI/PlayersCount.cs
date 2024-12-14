using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayersCount : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playersCountText;

    private NetworkVariable<int> playersCount = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Update()
    {
        playersCountText.text = "Player Count: " + playersCount.Value.ToString();

        if (!IsServer) return;
        playersCount.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }
}
