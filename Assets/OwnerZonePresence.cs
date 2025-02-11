using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class OwnerZonePresence : NetworkBehaviour
{
    [SerializeField] private bool ownerIsInZone = false;
    public ZoneHelper zoneHelper;
    private Zone zone;

    public Zone Zone
    {
        get => zone;
        set
        {
            if (zone != value)
            {
                zone = value;
                UpdateOwnerPresence();
            }
        }
    }

    [SerializeField] public UnityEvent onPresenceChange;


    public bool OwnerIsInSpawner => ownerIsInZone;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        zoneHelper = NetworkManager.Singleton.ConnectedClients[OwnerClientId].PlayerObject.GetComponent<ZoneHelper>();

        zoneHelper.onZoneChange.AddListener(UpdateOwnerPresence);
        UpdateOwnerPresence();
    }

    public override void OnNetworkDespawn()
    {
        if (!IsServer) return;

        zoneHelper.onZoneChange.RemoveListener(UpdateOwnerPresence);
    }

    public void UpdateOwnerPresence()
    {
        ownerIsInZone = (zoneHelper.Zone == zone);
        onPresenceChange.Invoke();
    }
}
