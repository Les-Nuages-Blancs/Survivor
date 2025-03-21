using System;
using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

public class TriggerStayEvent : NetworkBehaviour
{
    [SerializeField] private float requiredStayTime = 2f; // Time required to trigger event
    [SerializeField] private float updateTime = 0.1f; // Interval to emit time updates
    [SerializeField] private bool requireHost = true; // Interval to emit time updates

    private float timeInside = 0f;
    private bool isInside = false;
    private float nextUpdate = 0f;

    public event Action<float> OnTimeUpdated; // Emits the current time inside
    public event Action OnStayTimeReached; // Emits when time is reached
    public event Action OnExit;
    public event Action OnEnter;

    public Gate gate;

    private List<Collider> activater = new List<Collider>();

    private bool checkIsServer()
    {
        if (gate != null)
        {
            return gate.IsServer;
        }
        else
        {
            return IsServer;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!checkIsServer()) return;

        if (IsHostPlayer(other))
        {
            if (activater.Count == 0)
            {
                timeInside = 0f; // Reset timer
                nextUpdate = 0f; // Reset update time
                OnEnter?.Invoke();
            }

            isInside = true;
            activater.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!checkIsServer()) return;

        if (IsHostPlayer(other))
        {
            activater.Remove(other);

            if (activater.Count == 0)
            {
                isInside = false;
                timeInside = 0f; // Reset timer
                OnExit?.Invoke();
            } else
            {
                isInside = true;
            }
        }
    }

    private void Update()
    {
        if (!checkIsServer()) return;

        if (isInside)
        {
            timeInside += Time.deltaTime;

            // Emit update event at intervals
            if (timeInside >= nextUpdate)
            {
                OnTimeUpdated?.Invoke(timeInside);
                nextUpdate += updateTime;
            }

            if (timeInside >= requiredStayTime)
            {
                OnStayTimeReached?.Invoke();
                isInside = false; // Prevent multiple triggers
            }
        }
    }

    private bool IsHostPlayer(Collider other)
    {
        OnTriggerEnterForwarder forwarder = other.GetComponent<OnTriggerEnterForwarder>();
        if (forwarder)
        {
            GameObject target = forwarder.ForwardedGameObject;
            if (target != null)
            {
                GameObject localPlayer = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject()?.gameObject;
                if (localPlayer == target || !requireHost)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
