using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statLevelSystem;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject separatorPrefab;
    [SerializeField] private Transform healthBarSeparatorContainer;
    [SerializeField] private float separatorRange = 1000.0f;

    private void Start()
    {
        UpdateSeparators(healthSlider.maxValue);
    }

    public void UpdateHealthBar()
    {
        UpdateMaxHealth();
        SetHealth();
    }

    public void UpdateMaxHealth()
    {
        SetMaxHealth(statLevelSystem.BaseStatistiques.Health);
    }

    public void SetHealth()
    {
        healthSlider.value = healthSystem.CurrentHealth;
    }

    private void SetMaxHealth(float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        UpdateSeparators(maxHealth);
    }

    private void UpdateSeparators(float maxHealth)
    {
        if (!Application.isPlaying) return;
        foreach (Transform child in healthBarSeparatorContainer)
        {
            Destroy(child.gameObject);
        }

        var separatorCount = (int)maxHealth / separatorRange;
        if (separatorCount <= 0) return;
        for (var i = 0; i < separatorCount; i++)
        {
            var posX = 1000 * (i + 1) / maxHealth * -0.95f;
            Instantiate(separatorPrefab, healthBarSeparatorContainer).GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        }
    }
}
