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
            OnParentZoneChanged();
        }
        else
        {
            spawnerZone.OnParentZoneChanged.AddListener(OnParentZoneChanged);
            Debug.Log("task listener on parent zone change added");
        }
    }

    private void OnParentZoneChanged(Zone zone = null, Zone zone2 = null)
    {
        spawnerZone.ParentZone.AddTask(this);
        Debug.Log("task added");
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

    protected void triggerUpdate()
    {
        onUpgraderChange.Invoke();
    }
}
