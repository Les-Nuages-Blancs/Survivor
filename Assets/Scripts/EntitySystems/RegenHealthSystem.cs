using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class RegenHealthSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private NetworkVariable<float> regenSpeedInSecond = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<float> regenHealthValue = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] public UnityEvent onRegenSpeedInSecondChange;
    [SerializeField] public UnityEvent onRegenHealthValueChange;

    private Coroutine regenCoroutine;

    public void UpdateRegenStats()
    {
        if (!IsServer) return;
        regenHealthValue.Value = statsLevelSystem.BaseStatistiques.RegenHealth;
    }

    public override void OnNetworkSpawn()
    {
        regenHealthValue.OnValueChanged += OnRegenHealthValueChange;
        regenSpeedInSecond.OnValueChanged += OnRegenSpeedInSecondChange;

        UpdateRegenStats();
        StartRegen();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        regenHealthValue.OnValueChanged -= OnRegenHealthValueChange;
        regenSpeedInSecond.OnValueChanged -= OnRegenSpeedInSecondChange;
    }

    private void OnRegenSpeedInSecondChange(float oldValue, float newValue)
    {
        onRegenSpeedInSecondChange.Invoke();
    }

    private void OnRegenHealthValueChange(float oldValue, float newValue)
    {
        onRegenHealthValueChange.Invoke();
    }

    private void StartRegen()
    {
        if (!IsServer) return;
        regenCoroutine = StartCoroutine(ApplyRegen());
    }

    private void StopRegen()
    {
        if (!IsServer) return;

        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        }
    }

    private void OnDisable()
    {
        StopRegen();
    }

    private IEnumerator ApplyRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenSpeedInSecond.Value);
            healthSystem.AddHpServerRPC(regenHealthValue.Value);
        }
    }
}
