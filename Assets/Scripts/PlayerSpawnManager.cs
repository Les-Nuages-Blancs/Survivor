using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// TODO: Use RPC to ApllyPlayerSpawn

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    [SerializeField] private List<Transform> playerSpawnTransforms = new List<Transform>();
    private int playerTransformIndex = 0;

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

        Transform currentTransform = playerSpawnTransforms[playerTransformIndex];

        transform.position = currentTransform.position;
        transform.eulerAngles = currentTransform.eulerAngles;

        playerTransformIndex = (playerTransformIndex + 1) % playerSpawnTransforms.Count;
    }
}
