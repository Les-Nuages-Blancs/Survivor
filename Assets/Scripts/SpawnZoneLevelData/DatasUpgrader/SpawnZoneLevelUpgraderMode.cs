using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnZoneLevelUpgraderMode
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
    [SerializeField] private BaseSpawnZoneLevelData entityOperationStatistiques = new BaseSpawnZoneLevelData(0.0f, -0.1f, 1);

    public BaseSpawnZoneLevelData ApplyOperation(BaseSpawnZoneLevelData baseDatas, BaseSpawnZoneLevelData currentDatas)
    {
        BaseSpawnZoneLevelData operationTarget;
        switch (levelUpgraderOperationTarget)
        {
            case LevelUpgraderOperationTarget.Base:
                operationTarget = baseDatas;
                break;
            case LevelUpgraderOperationTarget.Current:
                operationTarget = currentDatas;
                break;
            default:
                operationTarget = new BaseSpawnZoneLevelData();
                Debug.LogWarning("no mode match the existing one, use of a default target operation stats");
                break;
        }

        BaseSpawnZoneLevelData newDatas = new BaseSpawnZoneLevelData();
        switch (levelUpgraderMode)
        {
            case LevelUpgraderMode.AddThenSet:
                newDatas = operationTarget + entityOperationStatistiques;
                break;
            case LevelUpgraderMode.MutliplyThenSet:
                newDatas = operationTarget * entityOperationStatistiques;
                break;
            case LevelUpgraderMode.MultiplyThenAddToCurrent:
                newDatas = (operationTarget * entityOperationStatistiques) + currentDatas;
                break;
        }

        return newDatas;
    }
}