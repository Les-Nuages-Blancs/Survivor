using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityLevelUpgrader
{
    [SerializeField]
    private List<EntityLevelUpgraderMode> entityLevelUpgraderModes = new List<EntityLevelUpgraderMode>
    {
        new EntityLevelUpgraderMode(),
    };

    public List<EntityLevelUpgraderMode> EntityLevelUpgraderModes => entityLevelUpgraderModes;
}
