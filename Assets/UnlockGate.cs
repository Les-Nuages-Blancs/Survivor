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

    private void Start()
    {
        SetupTriggerEvent(triggerEvent1, timerText1);
        SetupTriggerEvent(triggerEvent2, timerText2);
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
        gameObject.SetActive(false);
    
        UnlockGateClientRpc();
    }

    [ClientRpc]
    private void UnlockGateClientRpc()
    {
        gameObject.SetActive(false);
    }
}