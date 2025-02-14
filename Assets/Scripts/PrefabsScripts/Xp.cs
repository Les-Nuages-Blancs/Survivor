using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Xp : Pickable
{
    [SerializeField] private int xp = 10;

    protected override void Pickup(GameObject target)
    {
        StatistiquesLevelSystem statistiquesLevelSystem = target.GetComponent<StatistiquesLevelSystem>();
        if (statistiquesLevelSystem)
        {
            statistiquesLevelSystem.AddXpServerRPC((int)(xp * LevelStateManager.Instance.PlayerXpMultiplier));
        }

        base.Pickup(target);
    }
}