using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRelayUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private Button joinButton;
    [SerializeField] private RelayManager relayManager;

    private void Start()
    {
        joinButton.onClick.AddListener(() =>
        {
            relayManager.JoinRelay(joinCodeInput.text);
        });
    }
}
