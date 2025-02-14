using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBonus : Pickable
{
    [SerializeField] private int level = 1;

    protected override void Pickup(GameObject target)
    {
        StatistiquesLevelSystem statistiquesLevelSystem = target.GetComponent<StatistiquesLevelSystem>();
        if (statistiquesLevelSystem)
        {
            statistiquesLevelSystem.AddLevelAndResetXpServerRPC(level);
        }

        base.Pickup(target);
    }
}
