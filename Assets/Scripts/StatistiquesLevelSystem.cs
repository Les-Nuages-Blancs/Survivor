using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class StatistiquesLevelSystem : NetworkBehaviour
{
    [SerializeField] private EntityLevelStatistiquesSO entityLevelStatistiques;

    [SerializeField, Range(0, 100)] private NetworkVariable<int> currentLevel = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    [SerializeField] private NetworkVariable<int> currentXp = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    [SerializeField] private EntityBaseStatistiques baseStatistiques;

    [SerializeField] private UnityEvent onBaseStatsChange;

    public EntityBaseStatistiques BaseStatistiques => baseStatistiques;

    public int CurrentLevel
    {
        get => currentLevel.Value;
        set
        {
            if (currentLevel.Value != value)
            {
                currentLevel.Value = value;
                UpdateLevelStat();
            }
        }
    }

    public int CurrentXp
    {
        get => currentXp.Value;
        set
        {
            if (currentXp.Value != value)
            {
                currentXp.Value = value;
                TryLevelUp();
            }
        }
    }

    private void Start()
    {
        UpdateLevelStat();
        TryLevelUp();
    }

    private void OnValidate()
    {
        UpdateLevelStat();
        TryLevelUp();
    }

    private void UpdateLevelStat()
    {
        baseStatistiques = entityLevelStatistiques.GetStatsOfLevel(CurrentLevel);
        onBaseStatsChange?.Invoke();
    }

    private void TryLevelUp()
    {
        int levelUpXpCost = entityLevelStatistiques.GetXpRequiredForNextLevel(currentLevel.Value);
        if (currentXp.Value >= levelUpXpCost)
        {
            currentXp.Value -= levelUpXpCost;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentLevel += 1;
        TryLevelUp();
    }
}
