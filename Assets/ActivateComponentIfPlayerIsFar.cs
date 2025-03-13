using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ActivateComponentIfPlayerIsFar : NetworkBehaviour
{
    [SerializeField] private float farDistance = 10f; // Distance threshold
    [SerializeField] private Behaviour componentToActivate; // The MonoBehaviour to toggle
    [SerializeField] public float multiplier = 1.0f; // The MonoBehaviour to toggle


    private void Update()
    {
        if (!IsServer) return;

        if (NetworkManager.Singleton == null) return;

        bool allPlayersFar = true;

        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            GameObject player = client.PlayerObject?.gameObject;
            if (player != null && Vector3.Distance(player.transform.position, transform.position) < farDistance * multiplier)
            {
                allPlayersFar = false;
                break; // No need to check further
            }
        }

        if (componentToActivate != null)
        {
            componentToActivate.enabled = !allPlayersFar;
        }
    }
}
