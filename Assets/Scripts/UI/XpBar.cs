using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statLevelSystem;
    [SerializeField] private Slider xpSlider;
    [SerializeField] private GameObject separatorPrefab;
    [SerializeField] private Transform xpBarSeparatorContainer;
    [SerializeField] private int separatorRange = 1000;

    private void Start()
    {
        UpdateSeparators(xpSlider.maxValue);
    }

    public void UpdateXpBar()
    {
        UpdateMaxXp();
        SetXp();
    }

    public void UpdateMaxXp()
    {
        SetMaxXp(statLevelSystem.BaseStatistiques.RequiredXpForNextLevel);
    }

    public void SetXp()
    {
        xpSlider.value = statLevelSystem.CurrentXp;
    }

    private void SetMaxXp(float maxXp)
    {
        xpSlider.maxValue = maxXp;
        UpdateSeparators(maxXp);
    }

    private void UpdateSeparators(float maxXp)
    {
        if (!Application.isPlaying) return;
        foreach (Transform child in xpBarSeparatorContainer)
        {
            Destroy(child.gameObject);
        }

        var separatorCount = (int)maxXp / separatorRange;
        if (separatorCount <= 0) return;
        for (var i = 0; i < separatorCount; i++)
        {
            var posX = separatorRange * (i + 1) / maxXp * -0.95f;
            var sep = Instantiate(separatorPrefab, xpBarSeparatorContainer);
            sep.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
            sep.transform.localScale = new Vector3(1, 0.25f, 1);

        }
    }
}
