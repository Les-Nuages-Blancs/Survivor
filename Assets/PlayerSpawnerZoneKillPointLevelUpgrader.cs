using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnerZoneKillPointLevelUpgrader : TaskZone
{
    [SerializeField] private float killPointRequired = 100;
    private float killPoint = 0;

    [SerializeField] public UnityEvent onUpgrade;

    static private Dictionary<ulong, List<PlayerSpawnerZoneKillPointLevelUpgrader>> instanceDictionary = new();

    [SerializeField] public List<SpawnCondition> spawnConditions = new List<SpawnCondition>();

    public float KillPoint
    {
        get => killPoint;
        set
        {
            if (SpawnConditionOk())
            {
                if (killPoint != value)
                {
                    killPoint = value;

                    triggerUpdate();

                    Debug.Log("kill Point : " + killPoint);

                    if (killPointRequired <= killPoint)
                    {
                        onUpgrade.Invoke();
                    }
                }
            }
        }
    }

    public void ResetCount()
    {
        killPoint = 0;
    }

    public static void AddKillPoint(ulong clientId, float killPoint)
    {
        if (instanceDictionary.ContainsKey(clientId))
        {
            foreach (PlayerSpawnerZoneKillPointLevelUpgrader item in instanceDictionary[clientId])
            {
                item.KillPoint += killPoint;
            }
        }
    }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!instanceDictionary.ContainsKey(OwnerClientId))
        {
            instanceDictionary[OwnerClientId] = new List<PlayerSpawnerZoneKillPointLevelUpgrader>();
        }
        instanceDictionary[OwnerClientId].Add(this);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        instanceDictionary[OwnerClientId].Remove(this);
        if (instanceDictionary[OwnerClientId].Count == 0)
        {
            instanceDictionary.Remove(OwnerClientId);
        }
    }

    private bool SpawnConditionOk()
    {
        bool canSpawn = true;

        foreach (SpawnCondition spawnCondtion in spawnConditions)
        {
            if (!spawnCondtion.IsOk)
            {
                canSpawn = false;
                break;
            }
        }

        return canSpawn;
    }

    public override string ToTaskZoneString()
    {
        return $"Collect a total of {Mathf.Ceil(killPointRequired)} kill point (obtain by killing monsters) to upgrade zone to next level ({Mathf.Floor(killPoint)} / {Mathf.Ceil(killPointRequired)})";
    }
}
