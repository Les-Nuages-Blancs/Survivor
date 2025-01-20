using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelStateManager : NetworkBehaviour
{
    public static LevelStateManager Instance { get; private set; }

    [SerializeField] private Zone firstZone;
    [SerializeField] private NetworkVariable<bool> spawnEntity = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<bool> enableAutoAttack = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Zone FirstZone => firstZone;
    public bool SpawnEntity => spawnEntity.Value;
    public bool EnableAutoAttack => enableAutoAttack.Value;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Multiple LevelStateManager instances detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public override void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
