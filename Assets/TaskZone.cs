using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public abstract class TaskZone : NetworkBehaviour
{
    [SerializeField] private SpawnerZone spawnerZone;

    [SerializeField] public UnityEvent onUpgraderChange;

    abstract public string ToTaskZoneString();

    public override void OnNetworkSpawn()
    {
        spawnerZone.OnParentZoneChanged.AddListener(onParentZoneChanged);
        if (spawnerZone.ParentZone != null)
        {
            spawnerZone.ParentZone.AddTask(this);
        }
    }

    public override void OnNetworkDespawn()
    {
        spawnerZone.OnParentZoneChanged.RemoveListener(onParentZoneChanged);
        spawnerZone.ParentZone.RemoveTask(this);
    }

    private void onParentZoneChanged(Zone oldZone, Zone newZone)
    {
        if (oldZone != null)
        {
            spawnerZone.ParentZone.RemoveTask(this);
        }

        spawnerZone.ParentZone.AddTask(this);
    }
}
