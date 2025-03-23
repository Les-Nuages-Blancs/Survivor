using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Player : NetworkBehaviour
{
    public static List<Player> playerList { get; private set; } = new List<Player>();

    public static event UnityAction<ulong> onPlayerAdded;
    public static event UnityAction<ulong> onPlayerRemoved;

    private bool isReparented = false;

    public override void OnNetworkSpawn()
    {
        playerList.Add(this);
        onPlayerAdded?.Invoke(OwnerClientId); 
    }

    public override void OnNetworkDespawn()
    {
        playerList.Remove(this);
        onPlayerRemoved?.Invoke(OwnerClientId); 
    }

    private void Update()
    {
        if (!isReparented && IsSpawned)
        {
            isReparented = true;
            transform.SetParent(LevelStateManager.Instance.PlayerParent);
        }
    }

    public static Player GetPlayerByClientId(ulong clientId)
    {
        return playerList.Find(p => p.OwnerClientId == clientId);
    }
}
