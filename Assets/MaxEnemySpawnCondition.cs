using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxEnemySpawnCondition : SpawnCondition
{
    [SerializeField] private Zone zone;

    private void Start()
    {
        UpdateCondtion(true);
    }

    public void UpdateCondtion(bool newPresence)
    {
        isOk = zone.CanSpawnEnemyInZone();
    }
}
