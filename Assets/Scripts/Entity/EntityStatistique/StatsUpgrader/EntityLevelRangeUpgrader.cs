using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityLevelRangeUpgrader
{
    [SerializeField, Range(1, 100)] private int numberOfLevel = 10;
    [SerializeField] private EntityLevelUpgrader levelUpgrader = new EntityLevelUpgrader();

    public int NumberOfLevel => numberOfLevel;
    public EntityLevelUpgrader LevelUpgrader => levelUpgrader;
}
