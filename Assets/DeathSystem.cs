using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DeathSystem : NetworkBehaviour
{
    private bool isDespawning = false;

    public void TryKillProjectile()
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;
        DestroyServerRPC();
    }

    [ServerRpc]
    public void DestroyServerRPC()
    {
        if (isDespawning) return;
        isDespawning = true;

        NetworkObject.Despawn();
        Destroy(gameObject);
    }
}
