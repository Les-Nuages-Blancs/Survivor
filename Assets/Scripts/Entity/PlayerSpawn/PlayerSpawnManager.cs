using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSpawnManager : NetworkBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    [SerializeField] private List<Transform> playerSpawnTransforms = new List<Transform>();
    public int playerTransformIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ApplyPlayerSpawn(ApplyPlayerSpawn applyPlayerSpawn)
    {
        if (playerSpawnTransforms.Count == 0)
        {
            Debug.LogError("No spawn points assigned in PlayerSpawnManager.");
            return;
        }

        Transform currentTransform = playerSpawnTransforms[playerTransformIndex];

        applyPlayerSpawn.transform.position = currentTransform.position;
        applyPlayerSpawn.transform.eulerAngles = currentTransform.eulerAngles;

        applyPlayerSpawn.ApplyPlayerSpawnClientRPC(currentTransform.position, currentTransform.eulerAngles);

        playerTransformIndex = (playerTransformIndex + 1) % playerSpawnTransforms.Count;
    }
}
