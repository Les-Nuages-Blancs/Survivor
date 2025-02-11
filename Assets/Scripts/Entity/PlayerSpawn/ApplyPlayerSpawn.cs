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
        PlayerSpawnManager.Instance.ApplyPlayerSpawn(this);
    }

    [ClientRpc]
    public void ApplyPlayerSpawnClientRPC(Vector3 position, Vector3 eulerAngles)
    {
        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }
}
