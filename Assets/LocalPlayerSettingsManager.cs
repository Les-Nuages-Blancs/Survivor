using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class LocalPlayerSettingsManager : MonoBehaviour
{
    public static LocalPlayerSettingsManager Instance { get; private set; }

    [SerializeField] private string playerName = "default name";

    public UnityEvent<string> onPlayerNameChange;

    public string PlayerName
    {
        get => playerName;
        set
        {
            string newValue = Regex.Replace(value.Trim(), @"\s+", " ");
            
            if (playerName != newValue)
            {
                // TODO: check if another player as already this username

                if (newValue != null && newValue != "")
                {
                    onPlayerNameChange.Invoke(newValue);
                    playerName = newValue;
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple LocalPlayerSettingsManager instances detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
