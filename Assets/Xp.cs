using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Xp : NetworkBehaviour
{
    [SerializeField] private int xp = 10;
    [SerializeField] private GameObject xpPickupEffectPrefab;

    [TagField]
    [SerializeField] private List<string> pickupByTags = new List<string>();


    [SerializeField] public UnityEvent onXpPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (pickupByTags.Contains(target.tag))
            {
                StatistiquesLevelSystem statistiquesLevelSystem = target.GetComponent<StatistiquesLevelSystem>();
                if (statistiquesLevelSystem)
                {
                    statistiquesLevelSystem.AddXpServerRPC(xp);
                }
                SpawnParticleServerRPC();
                onXpPickup.Invoke();
            }
        }
    }

    [ServerRpc]
    private void SpawnParticleServerRPC()
    {
        if (xpPickupEffectPrefab)
        {
            GameObject go = Instantiate(xpPickupEffectPrefab, transform.position, transform.rotation);
            NetworkObject networkObject = go.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
}