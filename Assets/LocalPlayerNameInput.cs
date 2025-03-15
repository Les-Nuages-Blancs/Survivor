using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalPlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        inputField.onEndEdit.AddListener(UpdatePlayerName);
        LocalPlayerSettingsManager.Instance.onPlayerNameChange.AddListener(UpdateInputField);
    }

    private void OnDestroy()
    {
        inputField.onEndEdit.RemoveListener(UpdatePlayerName);
        LocalPlayerSettingsManager.Instance.onPlayerNameChange.RemoveListener(UpdateInputField);
    }

    private void UpdatePlayerName(string newName)
    {
        LocalPlayerSettingsManager.Instance.PlayerName = newName;
    }

    private void UpdateInputField(string newName)
    {
        if (inputField.text != newName)
        {
            inputField.text = newName;
        }
    }
}
