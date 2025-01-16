using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ApplyPlayerSpawn : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        Camera.main.GetComponent<CameraFollowSystem>().target = transform;

        ApplyPlayerSpawnServerRPC();
    }
    
    [ServerRpc]
    private void ApplyPlayerSpawnServerRPC()
    {
        PlayerSpawnManager.Instance.ApplyPlayerSpawn(transform);
    }
}
