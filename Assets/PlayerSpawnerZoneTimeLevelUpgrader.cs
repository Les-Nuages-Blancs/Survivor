using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpawnerZoneTimeLevelUpgrader : NetworkBehaviour
{
    [SerializeField] private float surviveTime = 10;

    [SerializeField] public UnityEvent onUpgrade;

    private Coroutine upgradeCoroutine;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        upgradeCoroutine = StartCoroutine(LaunchUpgrade());
    }

    public void StopUpgrade()
    {
        if (upgradeCoroutine != null)
        {
            StopCoroutine(upgradeCoroutine);
        }
    }

    private IEnumerator LaunchUpgrade()
    {
        while (true)
        {
            yield return new WaitForSeconds(surviveTime);

            onUpgrade.Invoke();
        }
    }
}
