using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// TODO: Use RPC to ApllyPlayerSpawn

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    [SerializeField] private List<Transform> playerSpawnTransforms = new List<Transform>();
    private NetworkVariable<int> playerTransformIndex = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyPlayerSpawn(Transform transform)
    {
        if (playerSpawnTransforms.Count == 0)
        {
            Debug.LogError("No spawn points assigned in PlayerSpawnManager.");
            return;
        }

        Transform currentTransform = playerSpawnTransforms[playerTransformIndex.Value];

        transform.position = currentTransform.position;
        transform.eulerAngles = currentTransform.eulerAngles;

        playerTransformIndex.Value = (playerTransformIndex.Value + 1) % playerSpawnTransforms.Count;
    }
}
