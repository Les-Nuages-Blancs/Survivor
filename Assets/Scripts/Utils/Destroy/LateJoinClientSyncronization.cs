using Unity.Netcode;
using UnityEngine;

public class LateJoinClientSynchronization : NetworkBehaviour
{
    public override void OnNetworkDespawn()
    {
        // Ensure the object is destroyed on clients if it is not synchronized with the server.
        if (!IsServer && !NetworkObject.IsSpawned)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (IsServer)
            return; // No need to do anything on the server.

        // Defer destruction until the client has fully connected.
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Ensure logic is only applied to the connecting client.
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            if (!NetworkObject.IsSpawned)
            {
                Destroy(gameObject);
            }

            // Unsubscribe after performing the check.
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        // Clean up the callback to avoid memory leaks.
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        }
    }
}
