using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnableWhenConnect : MonoBehaviour
{
    [SerializeField] private List<Behaviour> components = new List<Behaviour>();

    private void OnEnable()
    {
        StartCoroutine(WaitForNetworkManagerInitialization());
    }

    private IEnumerator WaitForNetworkManagerInitialization()
    {
        // Wait until the end of the frame to allow NetworkManager to be initialized
        yield return new WaitForEndOfFrame();

        // Check if the NetworkManager is available
        if (NetworkManager.Singleton == null)
        {
            Debug.LogError("NetworkManager.Singleton is not assigned. Ensure the NetworkManager is in the scene.");
            yield break;
        }

        // Now safely access NetworkManager.Singleton
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        UpdateVisibility();
    }

    private void OnDisable()
    {
        // Remove the listener when the object is disabled or destroyed
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        if (NetworkManager.Singleton.IsClient && NetworkManager.Singleton.IsConnectedClient)
        {
            EnableAllComponents();
        }
        else
        {
            DisableAllComponents();
        }
    }

    private void DisableAllComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }
    }

    private void EnableAllComponents()
    {
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
    }
}
