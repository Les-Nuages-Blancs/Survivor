using System;
using UnityEngine;
using Unity.Netcode;

public class TriggerStayEvent : NetworkBehaviour
{
    [SerializeField] private float requiredStayTime = 2f; // Time required to trigger event
    [SerializeField] private float updateTime = 0.1f; // Interval to emit time updates

    private float timeInside = 0f;
    private bool isInside = false;
    private float nextUpdate = 0f;

    public event Action<float> OnTimeUpdated; // Emits the current time inside
    public event Action OnStayTimeReached; // Emits when time is reached
    public event Action OnExit;
    public event Action OnEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;

        if (IsHostPlayer(other))
        {
            isInside = true;
            timeInside = 0f; // Reset timer
            nextUpdate = 0f; // Reset update time
            OnEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;

        if (IsHostPlayer(other))
        {
            isInside = false;
            timeInside = 0f; // Reset timer
            OnExit?.Invoke();
        }
    }

    private void Update()
    {
        if (!IsServer) return;

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
                if (localPlayer == target)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
