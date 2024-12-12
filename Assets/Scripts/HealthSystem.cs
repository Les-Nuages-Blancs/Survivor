using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
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
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
