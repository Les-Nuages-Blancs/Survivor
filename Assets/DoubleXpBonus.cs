using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleXpBonus : LimitedTimeBonus
{
    [SerializeField] private float XpMultiplierToAdd = 1.0f;

    override protected void onBonusStart()
    {
        PlayerBonusManager.Instance.PlayerXpMultiplier.Value += XpMultiplierToAdd;
    }

    override protected void onBonusStop()
    {
        PlayerBonusManager.Instance.PlayerXpMultiplier.Value -= XpMultiplierToAdd;
    }
}
