using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;

    private NetworkVariable<float> currentHealth = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    private NetworkVariable<float> maxHealth = new NetworkVariable<float>(100.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private float takeDamageCooldown = 0.5f;

    [SerializeField] public UnityEvent onNetworkSpawn;
    [SerializeField] public UnityEvent onHealthChange;
    [SerializeField] public UnityEvent onMaxHealthChange;
    [SerializeField] public UnityEvent onDeath;
    [SerializeField] public UnityEvent onTakeDamage;

    private bool isOnCooldown = false;
    public bool IsOnCooldown => isOnCooldown;


    public float CurrentHealth
    {
        get => currentHealth.Value;
        set
        {
            if (currentHealth.Value != value)
            {
                currentHealth.Value = Mathf.Clamp(value, 0, MaxHealth); ;
            }
        }
    }
    public float MaxHealth => maxHealth.Value;

    public override void OnNetworkSpawn()
    {
        currentHealth.OnValueChanged += OnHealthChanged;
        maxHealth.OnValueChanged += OnMaxHealthChanged;

        UpdateHealthStats();
        RegenAllHpServerRPC();
        onNetworkSpawn.Invoke();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        currentHealth.OnValueChanged -= OnHealthChanged;
        maxHealth.OnValueChanged -= OnMaxHealthChanged;
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        if (newValue == 0.0f)
        {
            onDeath.Invoke();
        }

        onHealthChange.Invoke();

    }

    private void OnMaxHealthChanged(float oldValue, float newValue)
    {
        onMaxHealthChange.Invoke();
    }

    public void UpdateHealthStats()
    {
        if (!IsServer) return;

        float newMaxHealth = statsLevelSystem.CurrentStatistiques.Health;

        if (MaxHealth != newMaxHealth)
        {
            UpdateMaxHealthServerRpc(newMaxHealth);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ClampHealthServerRPC()
    {
        float clampedHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        if (CurrentHealth != clampedHealth)
        {
            currentHealth.Value = clampedHealth;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateMaxHealthServerRpc(float newMaxHealth)
    {
        maxHealth.Value = newMaxHealth;
        ClampHealthServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddHpServerRPC(float hp)
    {
        CurrentHealth += hp;
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddHpByPercentOfMaxHpServerRPC(float maxHpPercent)
    {
        CurrentHealth += maxHpPercent * MaxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RegenAllHpServerRPC()
    {
        CurrentHealth = MaxHealth;
    }

    private void StartTakeDamageCooldown()
    {
        if (takeDamageCooldown == 0.0f) return;
        StartCoroutine(ApplyCooldown());
    }

    private IEnumerator ApplyCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(takeDamageCooldown);
        isOnCooldown = false;
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRPC(float damage)
    {
        if (isOnCooldown) return;
        StartTakeDamageCooldown();
        CurrentHealth -= damage;
        onTakeDamage.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageByPercentOfMaxHpServerRPC(float maxHpPercent)
    {
        if (isOnCooldown) return;
        StartTakeDamageCooldown();
        CurrentHealth -= maxHpPercent * MaxHealth;
        onTakeDamage.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveAllHpServerRPC()
    {
        CurrentHealth = 0;
    }
}
