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
    
    [SerializeField] private List<GameObject> visualsGo = new List<GameObject>();

    private void Awake()
    {
        SetVisuals(false);
    }

    public override void OnNetworkSpawn()
    {

        if (IsOwner)
        {
            Camera.main.GetComponent<CameraFollowSystem>().target = cameraFollowPoint != null ? cameraFollowPoint : transform;

            ApplyPlayerSpawnServerRPC();
        }
        else 
        {
            SetVisuals(true);
        }

    }

    public void SetVisuals(bool state)
    {
        foreach (GameObject go in visualsGo)
        {
            go.SetActive(state);
        }
    }

    [ServerRpc]
    private void ApplyPlayerSpawnServerRPC()
    {
        PlayerSpawnManager.Instance.ApplyPlayerSpawn(this);

        SetVisuals(true);
    }

    [ClientRpc]
    public void ApplyPlayerSpawnClientRPC(Vector3 position, Vector3 eulerAngles)
    {
        spawnPosition = position;
        transform.position = position;
        transform.eulerAngles = eulerAngles;

        SetVisuals(true);
    }
}
