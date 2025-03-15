using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("Unknown", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            UpdatePlayerName(LocalPlayerSettingsManager.Instance.PlayerName);
            LocalPlayerSettingsManager.Instance.onPlayerNameChange.AddListener(UpdatePlayerName);
        }

        UpdatePlayerNameLabel(networkPlayerName.Value.ToString());
        networkPlayerName.OnValueChanged += OnNetworkPlayerValueChanged;
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            LocalPlayerSettingsManager.Instance.onPlayerNameChange.RemoveListener(UpdatePlayerName);
        }

        networkPlayerName.OnValueChanged -= OnNetworkPlayerValueChanged;
    }

    private void UpdatePlayerName(string newName)
    {
        networkPlayerName.Value = newName;
    }

    private void OnNetworkPlayerValueChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        UpdatePlayerNameLabel(newValue.ToString());
    }

    private void UpdatePlayerNameLabel(string newValue)
    {
        playerName.text = newValue;
    }
}
