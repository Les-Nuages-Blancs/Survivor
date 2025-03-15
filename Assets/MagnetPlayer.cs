using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MagnetPlayer : NetworkBehaviour
{
    [SerializeField] private float magnetRange = 5f; // Range fixe du magnet
    [SerializeField] private float attractionSpeed = 10f; // Vitesse d’attraction
    [SerializeField] private SphereCollider magnetCollider;
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    private List<MagnetObject> objectsInRange = new();

    private void Awake()
    {
        // Ajoute un SphereCollider pour la détection
        magnetCollider = gameObject.AddComponent<SphereCollider>();
        magnetCollider.isTrigger = true;
        magnetCollider.radius = magnetRange;
    }

    public void UpdateMagnetStats()
    {
        if (!IsServer) return;

        float newMagnetRange = statsLevelSystem.CurrentStatistiques.PickupRange;

        if (magnetRange != newMagnetRange)
        {
            magnetRange = newMagnetRange;
            magnetCollider.radius = magnetRange;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;
            if (target != null)
            {
                MagnetObject magnetObject = target.GetComponent<MagnetObject>();
                if (magnetObject != null)
                {
                    objectsInRange.Add(magnetObject);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        MagnetObject magnetObject = other.GetComponent<MagnetObject>();
        if (magnetObject != null)
        {
            objectsInRange.Remove(magnetObject);
        }
    }

    private void Update()
    {
        for (int i = objectsInRange.Count - 1; i >= 0; i--)
        {
            MagnetObject magnetObject = objectsInRange[i];

            if (magnetObject == null)
            {
                objectsInRange.RemoveAt(i);
                continue;
            }

            if (magnetObject.IsBeingMagnetized)
            {
                objectsInRange.RemoveAt(i);
                continue;
            }

            float effectiveRange = magnetRange * magnetObject.MagnetMultiplier;
            float distance = Vector3.Distance(transform.position, magnetObject.transform.position);

            if (distance <= effectiveRange)
            {
                magnetObject.StartMagnet(this, attractionSpeed);
                objectsInRange.RemoveAt(i);
            }
        }
    }
}
