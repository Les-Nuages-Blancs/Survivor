using System.Collections;
using System.Collections.Generic;
using UI.Camera;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyPlayerSpawn : NetworkBehaviour
{
    [SerializeField] private Transform cameraFollowPoint;
    
    [DoNotSerialize] public Vector3 spawnPosition;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        Camera.main.GetComponent<CameraFollowSystem>().target = cameraFollowPoint != null ? cameraFollowPoint : transform;

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
        spawnPosition = position;
        transform.position = position;
        transform.eulerAngles = eulerAngles;
    }
}
