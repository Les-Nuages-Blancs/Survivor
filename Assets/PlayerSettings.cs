using System;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    // Ajout de l'événement pour notifier les changements de nom
    public event Action<string> OnPlayerNameChanged;

    private NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>(
        "Unknown",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

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
        string name = newValue.ToString();
        UpdatePlayerNameLabel(name);

        // Déclencher l'événement lorsqu'il y a un changement de nom
        OnPlayerNameChanged?.Invoke(name);
    }

    private void UpdatePlayerNameLabel(string newValue)
    {
        if (playerName != null)
        {
            playerName.text = newValue;
        }
    }

    public string GetPlayerName()
    {
        return networkPlayerName.Value.ToString();
    }
}
