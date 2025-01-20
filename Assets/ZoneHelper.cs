using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ZoneHelper : NetworkBehaviour
{
    [SerializeField] public Zone zone;

    public override void OnNetworkSpawn()
    {
        zone = LevelStateManager.Instance.FirstZone;
    }
}
