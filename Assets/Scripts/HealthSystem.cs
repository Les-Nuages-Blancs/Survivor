using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private NetworkVariable<float> currentHealth = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone);
    [SerializeField] private float maxHealth;

    [SerializeField] private UnityEvent onHealthChange;
    [SerializeField] private UnityEvent onMaxHealthChange;

#if UNITY_EDITOR
    private float _previousHealth = 0.0f;
#endif

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
        float newMaxHealth = statsLevelSystem.BaseStatistiques.Health;
        if (maxHealth != newMaxHealth)
        {
            maxHealth = newMaxHealth;
            onMaxHealthChange.Invoke();
            ClampHealth();
        }
    }

    private void ClampHealth()
    {
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);
#if UNITY_EDITOR
        _previousHealth = currentHealth.Value;
#endif
        onHealthChange.Invoke();
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

#if UNITY_EDITOR
        if (Application.isPlaying && currentHealth.Value != _previousHealth)
        {
            _previousHealth = currentHealth.Value;
            onHealthChange.Invoke();
        }
#endif
    }
}
