using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnZoneLevelRangeUpgrader
{

    [SerializeField, Range(1, 100)] private int numberOfLevel = 10;
    [SerializeField] private SpawnZoneLevelUpgrader levelUpgrader = new SpawnZoneLevelUpgrader();

    public int NumberOfLevel => numberOfLevel;
    public SpawnZoneLevelUpgrader LevelUpgrader => levelUpgrader;
}
