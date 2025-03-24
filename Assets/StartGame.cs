using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class StartGame : NetworkBehaviour
{
    [SerializeField] private TriggerStayEvent triggerEvent;
    [SerializeField] private TextMeshProUGUI timerText; // UI Text to update

    private void Start()
    {
        triggerEvent.OnTimeUpdated += UpdateText;
        triggerEvent.OnStayTimeReached += TimeReached;
        triggerEvent.OnExit += () => { timerText.gameObject.SetActive(false); };
        triggerEvent.OnEnter += () => { timerText.gameObject.SetActive(true); };
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateText(float time)
    {
        timerText.text = $"In: {time:F2}s";
    }

    private void TimeReached()
    {
        timerText.text = "Starting ...";
        gameObject.SetActive(false);
        GameNetworkManager.Instance.SyncrhoniseLocalShootSystemsClientRpc(); //error?
        LevelStateManager.Instance.SpawnEntity = true;
    }

    
}