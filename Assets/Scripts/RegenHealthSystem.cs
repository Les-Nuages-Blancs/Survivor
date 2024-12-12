using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenHealthSystem : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private HealthSystem healthSystem;

    [SerializeField] private float regenSpeedInSecond = 1.0f;
    [SerializeField] private float regenHealthValue;

    private Coroutine regenCoroutine;

    private void Start()
    {
        UpdateRegenStats();    
    }

    public void UpdateRegenStats()
    {
        regenHealthValue = statsLevelSystem.BaseStatistiques.RegenHealth;
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateRegenStats();
        }
    }

    private void OnEnable()
    {
        regenCoroutine = StartCoroutine(ApplyRegen());
    }

    private void OnDisable()
    {
        if (regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
        } 
    }

    private IEnumerator ApplyRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenSpeedInSecond);
            healthSystem.AddHp(regenHealthValue);
        }
    }
}
