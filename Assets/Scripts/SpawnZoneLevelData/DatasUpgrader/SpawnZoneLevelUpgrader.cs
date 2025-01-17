using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnZoneLevelUpgrader
{
    [SerializeField]
    private List<SpawnZoneLevelUpgraderMode> spawnZoneLevelUpgraderModes = new List<SpawnZoneLevelUpgraderMode>
{
    new SpawnZoneLevelUpgraderMode(),
};

    public List<SpawnZoneLevelUpgraderMode> SpawnZoneLevelUpgraderModes => spawnZoneLevelUpgraderModes;
}

