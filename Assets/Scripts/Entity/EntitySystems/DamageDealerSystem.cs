using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class DamageDealerSystem : NetworkBehaviour
{
    [SerializeField] public float damage = 5f;
    [SerializeField] private List<GameObject> EffectsPrefab = new List<GameObject>();

    [SerializeField] private LayerMask includeTriggerLayers;
    [TagField]
    [SerializeField] private List<string> includeDamageTags = new List<string>();


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

            if (EffectsPrefab != null)
            {
                SpawnParticleServerRPC();
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        bool shouldTrigger = false;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (includeDamageTags.Contains(target.tag))
            {
                shouldTrigger = true;

                HealthSystem healthSystem = target.GetComponent<HealthSystem>();
                if (healthSystem)
                {
                    healthSystemsInTrigger.Add(healthSystem);
                    ApplyDamageToHealthSystem(healthSystem);
                }
            }
        }
        else if ((includeTriggerLayers.value & (1 << other.gameObject.layer)) != 0) 
        { 
            shouldTrigger = true;
        }

        if (shouldTrigger) {
            onTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer || !NetworkObject.IsSpawned) return;

        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;

            if (includeDamageTags.Contains(target.tag))
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
        foreach( var effect in EffectsPrefab)
        {
            GameObject go = Instantiate(effect, transform.position, transform.rotation);

            DamageValueForward damageValueForward = go.GetComponent<DamageValueForward>();
            if (damageValueForward != null)
            {
                damageValueForward.damageValue = damage;
            }

            NetworkObject networkObject = go.GetComponent<NetworkObject>();
            networkObject.Spawn();
        }

    }
}