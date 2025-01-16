using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityLevelUpgraderMode
{
    public enum LevelUpgraderOperationTarget
    {
        Base,
        Current,
    }

    public enum LevelUpgraderMode
    {
        AddThenSet,
        MutliplyThenSet,
        MultiplyThenAddToCurrent
    }

    [SerializeField] private LevelUpgraderOperationTarget levelUpgraderOperationTarget = LevelUpgraderOperationTarget.Current;
    [SerializeField] private LevelUpgraderMode levelUpgraderMode = LevelUpgraderMode.AddThenSet;
    [SerializeField] private EntityBaseStatistiques entityOperationStatistiques = new EntityBaseStatistiques(50, 20.0f, 0.2f, 2.0f, 5.0f, 0.2f, 0.05f, 0.01f, 0.2f, 0.2f);

    public EntityBaseStatistiques ApplyOperation(EntityBaseStatistiques baseStats, EntityBaseStatistiques currentStats)
    {
        EntityBaseStatistiques operationTarget;
        switch (levelUpgraderOperationTarget)
        {
            case LevelUpgraderOperationTarget.Base:
                operationTarget = baseStats;
                break;
            case LevelUpgraderOperationTarget.Current:
                operationTarget = currentStats;
                break;
            default:
                operationTarget = new EntityBaseStatistiques();
                Debug.LogWarning("no mode match the existing one, use of a default target operation stats");
                break;
        }

        EntityBaseStatistiques newStats = new EntityBaseStatistiques();
        switch (levelUpgraderMode)
        {
            case LevelUpgraderMode.AddThenSet:
                newStats = operationTarget + entityOperationStatistiques;
                break;
            case LevelUpgraderMode.MutliplyThenSet:
                newStats = operationTarget * entityOperationStatistiques;
                break;
            case LevelUpgraderMode.MultiplyThenAddToCurrent:
                newStats = (operationTarget * entityOperationStatistiques) + currentStats;
                break;
        }

        return newStats;
    }
}
