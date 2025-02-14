using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBonus : Pickable
{
    [SerializeField] private float healthPercent = 0.3f;

    protected override void Pickup(GameObject target)
    {
        HealthSystem healthSystem = target.GetComponent<HealthSystem>();
        if (healthSystem)
        {
            healthSystem.AddHpByPercentOfMaxHpServerRPC(healthPercent);
        }

        base.Pickup(target);
    }
}
