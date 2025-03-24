using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class ZoneHelper : NetworkBehaviour
{
    [SerializeField] private Zone zone;
    [SerializeField] public UnityEvent onZoneChange;
    public UnityEvent<Zone, Zone> onZoneChangeDetails;

    public Zone Zone
    {
        get => zone;
        set
        {
            if (zone != value)
            {
                Zone oldZone = zone;
                zone = value;
                onZoneChangeDetails.Invoke(oldZone, value);
                onZoneChange.Invoke();
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        Zone = LevelStateManager.Instance.FirstZone;
    }
}
