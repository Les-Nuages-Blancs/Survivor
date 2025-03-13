using System.Collections;
using System.Collections.Generic;
using UI.Camera;
using Unity.Netcode;
using UnityEngine;

public class ApplyPlayerSpawn : NetworkBehaviour
{
    [SerializeField] private Transform cameraFollowPoint;
    [SerializeField] private List<GameObject> visualsGo = new List<GameObject>();

    public override void OnNetworkSpawn()
    {
        foreach (GameObject go in visualsGo)
        {
            go.SetActive(false);
        }

        if (!IsOwner) return;

        Camera.main.GetComponent<CameraFollowSystem>().target = cameraFollowPoint != null ? cameraFollowPoint : transform;

        ApplyPlayerSpawnServerRPC();
    }
    
    [ServerRpc]
    private void ApplyPlayerSpawnServerRPC()
    {
        PlayerSpawnManager.Instance.ApplyPlayerSpawn(this);

        foreach (GameObject go in visualsGo)
        {
            go.SetActive(true);
        }
    }

    [ClientRpc]
    public void ApplyPlayerSpawnClientRPC(Vector3 position, Vector3 eulerAngles)
    {
        transform.position = position;
        transform.eulerAngles = eulerAngles;

        foreach (GameObject go in visualsGo)
        {
            go.SetActive(true);
        }
    }
}
