using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[System.Serializable]
public class EntityBaseStatistiques: INetworkSerializable
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
    public float Armor => armor;
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float CritDamageMultiplier => critDamageMultiplier;
    public float CriticalChance => criticalChance;
    public float PickupRange => pickupRange;
    public float MoveSpeedMultiplier => moveSpeedMultiplier;

    public EntityBaseStatistiques()
    {
        requiredXpForNextLevel = 0;
        health = 0.0f;
        regenHealth = 0.0f;
        armor = 0.0f;
        damage = 0.0f;
        attackSpeed = 0.0f;
        critDamageMultiplier = 0.0f;
        criticalChance = 0.0f;
        pickupRange = 0.0f;
        moveSpeedMultiplier = 0.0f;
    }

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

    // Implementing the INetworkSerializable interface
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref requiredXpForNextLevel);
        serializer.SerializeValue(ref health);
        serializer.SerializeValue(ref regenHealth);
        serializer.SerializeValue(ref armor);
        serializer.SerializeValue(ref damage);
        serializer.SerializeValue(ref attackSpeed);
        serializer.SerializeValue(ref critDamageMultiplier);
        serializer.SerializeValue(ref criticalChance);
        serializer.SerializeValue(ref pickupRange);
        serializer.SerializeValue(ref moveSpeedMultiplier);
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

    public override bool Equals(object obj)
    {
        if (obj is EntityBaseStatistiques other)
        {
            return requiredXpForNextLevel == other.requiredXpForNextLevel &&
                   Mathf.Approximately(health, other.health) &&
                   Mathf.Approximately(regenHealth, other.regenHealth) &&
                   Mathf.Approximately(armor, other.armor) &&
                   Mathf.Approximately(damage, other.damage) &&
                   Mathf.Approximately(attackSpeed, other.attackSpeed) &&
                   Mathf.Approximately(critDamageMultiplier, other.critDamageMultiplier) &&
                   Mathf.Approximately(criticalChance, other.criticalChance) &&
                   Mathf.Approximately(pickupRange, other.pickupRange) &&
                   Mathf.Approximately(moveSpeedMultiplier, other.moveSpeedMultiplier);
        }
        return false;
    }

}
