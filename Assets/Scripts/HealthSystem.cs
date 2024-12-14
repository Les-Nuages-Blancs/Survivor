using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private NetworkVariable<float> currentHealth = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone);
    [SerializeField] private float maxHealth;

    public float CurrentHealth
    {
        get => currentHealth.Value;
        set
        {
            if (currentHealth.Value != value)
            {
                currentHealth.Value = value;
                ClampHealth();
            }
        }
    }

    private void Start()
    {
        UpdateHealthStats();
    }

    public void UpdateHealthStats()
    {
        maxHealth = statsLevelSystem.BaseStatistiques.Health;
        ClampHealth();
    }

    private void ClampHealth()
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
    }

    public void AddHp(float hp)
    {
        CurrentHealth += hp;
    }

    public void RegenAllHp()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateHealthStats();
        }
    }
}
