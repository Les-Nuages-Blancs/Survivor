using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DamageDealerSystem : NetworkBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private GameObject damageDealedEffectPrefab;

    [TagField]
    [SerializeField] private List<string> excludeDamageTags = new List<string>();

    [SerializeField] public UnityEvent onTriggerEnter;
    [SerializeField] public UnityEvent onTriggerExit;

    private List<HealthSystem> healthSystemsInTrigger = new List<HealthSystem>();

    private void FixedUpdate()
    {
        ApplyDamageToHealthSystems();
    }

    private void ApplyDamageToHealthSystems()
    {
        if (!IsServer) return;

        foreach (HealthSystem healthSystem in healthSystemsInTrigger)
        {
            ApplyDamageToHealthSystem(healthSystem);
        }
    }

    private void ApplyDamageToHealthSystem(HealthSystem healthSystem)
    {
        if (healthSystem && !healthSystem.IsOnCooldown)
        {
            healthSystem.TakeDamageServerRPC(damage);

            if (damageDealedEffectPrefab != null)
            {
                SpawnParticleServerRPC();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (!excludeDamageTags.Contains(target.tag))
            {
                HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                if (healthSystem)
                {
                    healthSystemsInTrigger.Add(healthSystem);
                    ApplyDamageToHealthSystem(healthSystem);
                }
            }
        }

        onTriggerEnter.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (!excludeDamageTags.Contains(target.tag))
            {
                HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                if (healthSystem)
                {
                    healthSystemsInTrigger.Remove(healthSystem);
                }
            }
        }

        onTriggerExit.Invoke();
    }

    [ServerRpc]
    private void SpawnParticleServerRPC()
    {
        GameObject go = Instantiate(damageDealedEffectPrefab, transform.position, transform.rotation);
        go.GetComponent<DamageValueForward>().SetDamageValue(damage);
        NetworkObject networkObject = go.GetComponent<NetworkObject>();
        networkObject.Spawn();
    }
}