using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnZoneLevelRangeGroup
{
    public List<SpawnZoneLevelRangeUpgrader> levelRangeUpgraders = new List<SpawnZoneLevelRangeUpgrader> {
        new SpawnZoneLevelRangeUpgrader()
    };
}