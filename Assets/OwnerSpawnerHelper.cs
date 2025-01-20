using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class OwnerSpawnerHelper : NetworkBehaviour
{
    [SerializeField] private GameObject owner;
    [SerializeField] private float minDistance = 5.0f;
    [SerializeField] private float maxDistance = 10.0f;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        owner = NetworkManager.Singleton.LocalClient.PlayerObject.gameObject;
    }

    public void UpdateSpawnPosition()
    {
        ZoneHelper zoneHelper = owner.GetComponent<ZoneHelper>();

        Vector3 spawnPosition = owner.transform.position;

        if (zoneHelper != null && zoneHelper.Zone != null)
        {
            spawnPosition = zoneHelper.Zone.GetRandomPositionOnNavMesh(owner.transform, minDistance, maxDistance);
        }

        transform.position = spawnPosition;
    }
}
