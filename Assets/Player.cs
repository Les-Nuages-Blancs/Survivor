using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public static List<GameObject> playerList { get; private set; } = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        playerList.Add(gameObject);
    }

    public override void OnNetworkDespawn()
    {
        playerList.Remove(gameObject);
    }
}
