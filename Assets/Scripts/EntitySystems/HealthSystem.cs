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

    [SerializeField] public UnityEvent onHealthChange;
    [SerializeField] public UnityEvent onMaxHealthChange;

    private bool isOnCooldown = false;
    public bool IsOnCooldown => isOnCooldown;

    [Tooltip("If not empty, gameObject will be destroyed and lootTable processed when hp goes below 1")]
    [SerializeField] public LootTable lootTable;

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
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        currentHealth.OnValueChanged -= OnHealthChanged;
        maxHealth.OnValueChanged -= OnMaxHealthChanged;
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        onHealthChange.Invoke();
        if(newValue < 1 && lootTable != null){
            LootManager.Instance.ProcessLootTable(lootTable, transform.position);
            Destroy(gameObject);
        }
    }

    private void OnMaxHealthChanged(float oldValue, float newValue)
    {
        onMaxHealthChange.Invoke();
    }

    public void UpdateHealthStats()
    {
        if (!IsServer) return;

        float newMaxHealth = statsLevelSystem.BaseStatistiques.Health;

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
    }

    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageByPercentOfMaxHpServerRPC(float maxHpPercent)
    {
        if (isOnCooldown) return;
        StartTakeDamageCooldown();
        CurrentHealth -= maxHpPercent * MaxHealth;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveAllHpServerRPC()
    {
        CurrentHealth = 0;
    }
}
