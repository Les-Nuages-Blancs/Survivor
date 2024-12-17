using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class StatistiquesLevelSystem : NetworkBehaviour
{
    [SerializeField] private EntityLevelStatistiquesSO entityLevelStatistiques;

    [SerializeField] private int currentLevel = 0;
    [SerializeField] private int currentXp = 0;

    [SerializeField] private EntityBaseStatistiques baseStatistiques;

    [SerializeField] private UnityEvent onBaseStatsChange;

    public EntityBaseStatistiques BaseStatistiques => baseStatistiques;

    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            if (currentLevel != value)
            {
                currentLevel = value;
                UpdateLevelStat();
            }
        }
    }

    public int CurrentXp
    {
        get => currentXp;
        set
        {
            if (currentXp != value)
            {
                currentXp = value;
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
        if (Application.isPlaying)
        {
            onBaseStatsChange?.Invoke();
        }
    }

    private void TryLevelUp()
    {
        int levelUpXpCost = entityLevelStatistiques.GetXpRequiredForNextLevel(currentLevel);
        if (currentXp >= levelUpXpCost)
        {
            currentXp -= levelUpXpCost;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        CurrentLevel += 1;
        TryLevelUp();
    }
}
