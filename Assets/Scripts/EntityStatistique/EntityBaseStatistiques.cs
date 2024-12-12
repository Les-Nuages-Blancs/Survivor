using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityBaseStatistiques
{
    [SerializeField, Range(0.0f, 5000.0f)] private int requiredXpForNextLevel = 0;
    [SerializeField, Range(0.0f, 5000.0f)] private float health = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float regenHealth = 0.0f;
    [SerializeField, Range(0.0f, 5000.0f)] private float armor = 0.0f;
    [SerializeField, Range(0.0f, 5000.0f)] private float damage = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float attackSpeed = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float critDamageMultiplier = 0.0f;
    [SerializeField, Range(0.0f, 1.0f)] private float criticalChance = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float pickupRange = 0.0f;
    [SerializeField, Range(0.0f, 100.0f)] private float moveSpeedMultiplier = 0.0f;

    public int RequiredXpForNextLevel => requiredXpForNextLevel;
    public float Health => health;
    public float RegenHealth => regenHealth;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float CritDamageMultiplier => critDamageMultiplier;
    public float CriticalChance => criticalChance;
    public float PickupRange => pickupRange;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;

    public EntityBaseStatistiques(
        int initRequiredXpForNextLevel = 0,
        float initHealth = 0.0f,
        float initRegenHealth = 0.0f,
        float initArmor = 0.0f,
        float initDamage = 0.0f,
        float initAttackSpeed = 0.0f,
        float initCritDamageMultiplier = 0.0f,
        float initCriticalChance = 0.0f,
        float initPickupRange = 0.0f,
        float initMoveSpeedMultiplier = 0.0f
    )
    {
        requiredXpForNextLevel = initRequiredXpForNextLevel;
        health = initHealth;
        regenHealth = initRegenHealth;
        armor = initArmor;
        damage = initDamage;
        attackSpeed = initAttackSpeed;
        critDamageMultiplier = initCritDamageMultiplier;
        criticalChance = initCriticalChance;
        pickupRange = initPickupRange;
        moveSpeedMultiplier = initMoveSpeedMultiplier;
    }

    // Addition operator
    public static EntityBaseStatistiques operator +(EntityBaseStatistiques a, EntityBaseStatistiques b)
    {
        return new EntityBaseStatistiques(
            a.requiredXpForNextLevel + b.requiredXpForNextLevel,
            a.health + b.health,
            a.regenHealth + b.regenHealth,
            a.armor + b.armor,
            a.damage + b.damage,
            a.attackSpeed + b.attackSpeed,
            a.critDamageMultiplier + b.critDamageMultiplier,
            a.criticalChance + b.criticalChance,
            a.pickupRange + b.pickupRange,
            a.moveSpeedMultiplier + b.moveSpeedMultiplier
        );
    }

    // Multiplication operator
    public static EntityBaseStatistiques operator *(EntityBaseStatistiques a, EntityBaseStatistiques b)
    {
        return new EntityBaseStatistiques(
            a.requiredXpForNextLevel * b.requiredXpForNextLevel,
            a.health * b.health,
            a.regenHealth * b.regenHealth,
            a.armor * b.armor,
            a.damage * b.damage,
            a.attackSpeed * b.attackSpeed,
            a.critDamageMultiplier * b.critDamageMultiplier,
            a.criticalChance * b.criticalChance,
            a.pickupRange * b.pickupRange,
            a.moveSpeedMultiplier * b.moveSpeedMultiplier
        );
    }

    // Clone method
    public EntityBaseStatistiques Clone()
    {
        return new EntityBaseStatistiques(
            requiredXpForNextLevel,
            health,
            regenHealth,
            armor,
            damage,
            attackSpeed,
            critDamageMultiplier,
            criticalChance,
            pickupRange,
            moveSpeedMultiplier
        );
    }
}
