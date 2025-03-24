using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class LocalPlayerUpgradeManager : MonoBehaviour
{
    public static LocalPlayerUpgradeManager Instance { get; private set; }

    [SerializeField] private EntityBaseStatistiques statsUpgraderLevel = new EntityBaseStatistiques();

    [SerializeField] private List<EntityBaseStatistiques> statsMultiplierBasedOnLevels = new List<EntityBaseStatistiques>();
    [SerializeField] private List<EntityBaseStatistiques> statsAdditiveBonusBasedOnLevels = new List<EntityBaseStatistiques>();

    public int railgunLvl = 0;
    public int lavaflaskLvl = 0;
    public UnityEvent<EntityBaseStatistiques, EntityBaseStatistiques> onStatsUpgraderLevelChange;

    public EntityBaseStatistiques StatsUpgraderLevel
    {
        get => statsUpgraderLevel;
        set
        {
            if (statsUpgraderLevel != value)
            {
                statsUpgraderLevel = value;
                onStatsUpgraderLevelChange.Invoke(GetCurrentStatsMultiplier(), GetCurrentStatsAdditiveBonus());
            }
        }
    }

    private bool CanUpgrade(int level)
    {
        return (level < statsMultiplierBasedOnLevels.Count && statsMultiplierBasedOnLevels.Count == statsAdditiveBonusBasedOnLevels.Count);
    }

    public void UpgradeHealth()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.Health + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradeHealthRegen()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.RegenHealth + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradeArmor()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.Armor + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradeDamage()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.Damage + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.AttackSpeed + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradePickupRange()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.PickupRange + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public void UpgradeMoveSpeedMultiplier()
    {
        if (CanUpgrade((int)(statsUpgraderLevel.MoveSpeedMultiplier + 1.0f)))
        {
            EntityBaseStatistiques upgrader = new EntityBaseStatistiques(0, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);

            EntityBaseStatistiques newStatsUpgrader = statsUpgraderLevel + upgrader;

            StatsUpgraderLevel = newStatsUpgrader;
        }
    }

    public EntityBaseStatistiques GetCurrentStatsMultiplier()
    {
        int currentRequiredXpForNextLevel = statsMultiplierBasedOnLevels[statsUpgraderLevel.RequiredXpForNextLevel].RequiredXpForNextLevel;
        float currentHealth = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.Health].Health;
        float currentRegenHealth = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.RegenHealth].RegenHealth;
        float currentArmor = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.Armor].Armor;
        float currentDamage = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.Damage].Damage;
        float currentAttackSpeed = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.AttackSpeed].AttackSpeed;
        float currentCritDamageMultiplier = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.CritDamageMultiplier].CritDamageMultiplier;
        float currentCriticalChance = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.CriticalChance].CriticalChance;
        float currentPickupRange = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.PickupRange].PickupRange;
        float currentMoveSpeedMultiplier = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.MoveSpeedMultiplier].MoveSpeedMultiplier;
        float currentKillPoint = statsMultiplierBasedOnLevels[(int)statsUpgraderLevel.KillPoint].KillPoint;

        EntityBaseStatistiques currentStats = new EntityBaseStatistiques(
            currentRequiredXpForNextLevel,
            currentHealth,
            currentRegenHealth,
            currentArmor,
            currentDamage,
            currentAttackSpeed,
            currentCritDamageMultiplier,
            currentCriticalChance,
            currentPickupRange,
            currentMoveSpeedMultiplier,
            currentKillPoint
        );

        return currentStats;
    }

    public EntityBaseStatistiques GetCurrentStatsAdditiveBonus()
    {
        int currentRequiredXpForNextLevel = statsAdditiveBonusBasedOnLevels[statsUpgraderLevel.RequiredXpForNextLevel].RequiredXpForNextLevel;
        float currentHealth = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.Health].Health;
        float currentRegenHealth = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.RegenHealth].RegenHealth;
        float currentArmor = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.Armor].Armor;
        float currentDamage = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.Damage].Damage;
        float currentAttackSpeed = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.AttackSpeed].AttackSpeed;
        float currentCritDamageMultiplier = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.CritDamageMultiplier].CritDamageMultiplier;
        float currentCriticalChance = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.CriticalChance].CriticalChance;
        float currentPickupRange = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.PickupRange].PickupRange;
        float currentMoveSpeedMultiplier = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.MoveSpeedMultiplier].MoveSpeedMultiplier;
        float currentKillPoint = statsAdditiveBonusBasedOnLevels[(int)statsUpgraderLevel.KillPoint].KillPoint;

        EntityBaseStatistiques currentStats = new EntityBaseStatistiques(
            currentRequiredXpForNextLevel,
            currentHealth,
            currentRegenHealth,
            currentArmor,
            currentDamage,
            currentAttackSpeed,
            currentCritDamageMultiplier,
            currentCriticalChance,
            currentPickupRange,
            currentMoveSpeedMultiplier,
            currentKillPoint
        );

        return currentStats;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple LocalPlayerSettingsManager instances detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
