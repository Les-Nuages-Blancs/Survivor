using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Pickable : NetworkBehaviour
{
    [SerializeField] private GameObject pickupEffectPrefab;

    [TagField]
    [SerializeField] private List<string> pickupByTags = new List<string>();


    [SerializeField] public UnityEvent onPickup;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;
            if (pickupByTags.Contains(target.tag))
            {
                Pickup(target);
            }
        }
    }

    virtual protected void Pickup(GameObject target)
    {
        SpawnParticleServerRPC();
        onPickup.Invoke();
    }

    [ServerRpc]
    private void SpawnParticleServerRPC()
    {
        if (pickupEffectPrefab)
        {
            GameObject go = Instantiate(pickupEffectPrefab, transform.position, transform.rotation);
            NetworkObject networkObject = go.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }
    }
}