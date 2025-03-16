using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnerZoneKillLevelUpgrader : TaskZone
{
    [SerializeField] private int killNumberRequired = 100;
    private int killNumber = 0;

    [SerializeField] public UnityEvent onUpgrade;

    static private Dictionary<ulong, List<PlayerSpawnerZoneKillLevelUpgrader>> instanceDictionary = new();

    [SerializeField] public List<SpawnCondition> spawnConditions = new List<SpawnCondition>();

    public int KillNumber
    {
        get => killNumber;
        set
        {
            if (SpawnConditionOk())
            {
                if (killNumber != value)
                {
                    killNumber = value;

                    onUpgraderChange.Invoke();

                    Debug.Log("kill Number : " + killNumber);

                    if (killNumberRequired <= killNumber)
                    {
                        onUpgrade.Invoke();
                    }
                }
            }
        }
    }
    public void ResetCount()
    {
        killNumber = 0;
    }

    public static void AddKillNumber(ulong clientId, int killNumber)
    {
        foreach (PlayerSpawnerZoneKillLevelUpgrader item in instanceDictionary[clientId])
        {
            item.KillNumber += killNumber;
        }
    }

    public static void AddKillNumber(ulong clientId)
    {
        AddKillNumber(clientId, 1);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!instanceDictionary.ContainsKey(OwnerClientId))
        {
            instanceDictionary[OwnerClientId] = new List<PlayerSpawnerZoneKillLevelUpgrader>();
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
        return $"Kill a total of {killNumberRequired} monsters to upgrade zone to next level ({KillNumber} / {killNumberRequired})";
    }
}
