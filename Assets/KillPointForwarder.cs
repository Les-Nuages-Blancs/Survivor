using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KillPointForwarder : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statistiqueLevelSystem;
    public void ForwardKillPoint()
    {
        PlayerSpawnerZoneKillPointLevelUpgrader.AddKillPoint(OwnerClientId, statistiqueLevelSystem.BaseStatistiques.KillPoint);
    }
}
