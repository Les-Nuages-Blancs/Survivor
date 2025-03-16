using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class LevelStateManager : NetworkBehaviour
{
    public static LevelStateManager Instance { get; private set; }

    [Header("Game Setup")]
    [SerializeField] private Zone firstZone;

    [Header("Parents")]
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform projectileParent;
    [SerializeField] private Transform playerParent;
    [SerializeField] private Transform xpParent;
    [SerializeField] private Transform bonusParent;
    [SerializeField] private Transform otherParent;
    [SerializeField] private Transform localParent; //a local GO is requiered to prevent warning when closing app

    [Header("Debug Tools")]
    [SerializeField] private NetworkVariable<bool> spawnEntity = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<bool> enableAutoAttack = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [Header("Outils de triche")]
    [SerializeField] private NetworkVariable<float> playerSpeedMultiplier = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<float> playerXpMultiplier = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<float> playerFireRateMultiplier = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<float> playerDamageMultiplier = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private NetworkVariable<float> enemySpawnSpeedMultiplier = new NetworkVariable<float>(1.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public Zone FirstZone => firstZone;

    public int totalNumberOfEnemy = 0;

    public Transform EnemyParent => enemyParent != null ? enemyParent : transform;
    public Transform ProjectileParent => projectileParent != null ? projectileParent : transform;
    public Transform PlayerParent => playerParent != null ? playerParent : transform;
    public Transform XpParent => xpParent != null ? xpParent : transform;
    public Transform BonusParent => bonusParent != null ? bonusParent : transform;
    public Transform OtherParent => otherParent != null ? otherParent : transform;
    public Transform LocalParent => localParent != null ? localParent : transform;

    public UnityEvent onSpawnEntityChanged;

    public bool SpawnEntity
    {
        get => spawnEntity.Value;
        set
        {
            spawnEntity.Value = value;
            onSpawnEntityChanged.Invoke();
        }
    }
    public bool EnableAutoAttack => enableAutoAttack.Value;

    public float PlayerSpeedMultiplier => playerSpeedMultiplier.Value;
    public float PlayerXpMultiplier => playerXpMultiplier.Value;
    public float PlayerFireRateMultiplier => playerFireRateMultiplier.Value;
    public float PlayerDamageMultiplier => playerDamageMultiplier.Value;
    public float EnemySpawnSpeedMultiplier => enemySpawnSpeedMultiplier.Value;

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
