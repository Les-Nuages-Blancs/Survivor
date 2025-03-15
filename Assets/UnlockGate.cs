using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class UnlockGate : NetworkBehaviour
{
    [SerializeField] private TriggerStayEvent triggerEvent1;
    [SerializeField] private TriggerStayEvent triggerEvent2;
    [SerializeField] private TextMeshProUGUI timerText1; // UI Text to update
    [SerializeField] private TextMeshProUGUI timerText2; // UI Text to update
    [SerializeField] private Zone zone;

    private void Start()
    {
        SetupTriggerEvent(triggerEvent1, timerText1);
        SetupTriggerEvent(triggerEvent2, timerText2);
        if (zone != null )
        {
            zone.onUnlock.AddListener(HideGate);
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        
        if (zone != null )
        {
            zone.onUnlock.RemoveListener(HideGate);
        }
    }

    private void HideGate()
    {
        gameObject.SetActive(false);
    }

    private void SetupTriggerEvent(TriggerStayEvent triggerEvent, TextMeshProUGUI timerText)
    {
        triggerEvent.OnTimeUpdated += (float time) => { UpdateText(time, timerText); };
        triggerEvent.OnStayTimeReached += () => { TimeReached(timerText); };
        triggerEvent.OnExit += () => { timerText.gameObject.SetActive(false); };
        triggerEvent.OnEnter += () => { timerText.gameObject.SetActive(true); };
    }

    private void UpdateText(float time, TextMeshProUGUI timerText)
    {
        timerText.text = $"In: {time:F2}s";
    }

    private void TimeReached(TextMeshProUGUI timerText)
    {
        timerText.text = "Unlocking ...";
        UnlockGateServerRpc();
    }

    [ServerRpc]
    private void UnlockGateServerRpc()
    {
        if (zone != null)
        {
            zone.IsUnlock = true;

            UnlockGateClientRpc();
        }
    }

    [ClientRpc]
    private void UnlockGateClientRpc()
    {
        zone.IsUnlock = true;
    }
}