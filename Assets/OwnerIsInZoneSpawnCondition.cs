using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerIsInZoneSpawnCondition : SpawnCondition
{
    [SerializeField] private OwnerZonePresence ownerZonePresence;

    private void Start()
    {
        ownerZonePresence.onPresenceChange.AddListener(UpdateCondtion);
        UpdateCondtion(true);
    }

    private void UpdateCondtion(bool newPresence)
    {
        isOk = ownerZonePresence.OwnerIsInSpawner;
    }
}
