using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statLevelSystem;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject separatorPrefab;
    [SerializeField] private Transform healthBarSeparatorContainer;

    private void Start()
    {
        UpdateSeparators(healthSlider.maxValue);
    }

    public void UpdateMaxHealth()
    {
        SetMaxHealth(statLevelSystem.BaseStatistiques.Health);
    }

    private void Update()
    {
        SetHealth(healthSystem.CurrentHealth);
    }

    private void SetHealth(float health)
    {
        healthSlider.value = health;
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

        var separatorCount = (int)maxHealth / 1000;
        if (separatorCount <= 0) return;
        for (var i = 0; i < separatorCount; i++)
        {
            var posX = 1000 * (i + 1) / maxHealth * -0.95f;
            Instantiate(separatorPrefab, healthBarSeparatorContainer).GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
        }
    }
}
