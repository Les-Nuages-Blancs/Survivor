using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MovementSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField]
    private NetworkVariable<float> moveSpeed =
        new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] public UnityEvent onMoveSpeedChange;

    [Header("Speed Configuration")]
    [SerializeField] private float minSpeedDistance = 10f;  // Min distance for speed lerp
    [SerializeField] private float maxSpeedDistance = 50f;  // Max distance for speed lerp
    [SerializeField] private float minSpeedMultiplier = 1f;  // Min speed multiplier
    [SerializeField] private float maxSpeedMultiplier = 2f;  // Max speed multiplier
    [SerializeField] private float lerpCurrentSpeedTime = 0.1f;
    public float lerpSpeedPercentage = 0f;
    public float distanceToTarget = 1f;

    [Header("Update Configuration")]
    [SerializeField] private float minUpdateDistance = 5f;  // Min distance for update lerp
    [SerializeField] private float maxUpdateDistance = 50f;  // Max distance for update lerp
    [SerializeField] private float minUpdateInterval = 1f;  // Min update interval in seconds
    [SerializeField] private float maxUpdateInterval = 2f;  // Max update interval in seconds

    private float lastPathUpdateTime = 0f;  // Track last path update time

    public Vector3? targetPosition = null;

    [Header("ToRemove - Debug Agent")]
    [SerializeField] private bool hasPath = false;
    [SerializeField] private bool isPending = false;
    [SerializeField] private float remainingDistance = 0f;
    

    public override void OnNetworkSpawn()
    {
        moveSpeed.OnValueChanged += OnMoveSpeedChange;
        UpdateMoveStats();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        moveSpeed.OnValueChanged -= OnMoveSpeedChange;
    }

    private void OnMoveSpeedChange(float oldValue, float newValue)
    {
        agent.speed = newValue;
        onMoveSpeedChange.Invoke();
    }

    public float MoveSpeed
    {
        get => moveSpeed.Value;
        set
        {
            if (moveSpeed.Value != value)
            {
                moveSpeed.Value = value;
                agent.speed = value;
            }
        }
    }

    public void UpdateMoveStats()
    {
        if (!IsServer) return;

        float baseSpeed = statsLevelSystem.BaseStatistiques.MoveSpeedMultiplier;
        float targetSpeed = baseSpeed;

        if (targetPosition.HasValue && agent.hasPath)
        {
            // Calculate remaining distance to target
            distanceToTarget = agent.remainingDistance;

            if (distanceToTarget != Mathf.Infinity)
            {
                lerpSpeedPercentage = Mathf.InverseLerp(minSpeedDistance, maxSpeedDistance, distanceToTarget);
            }
            else
            {
                lerpSpeedPercentage = 1f;
            }

            // Lerp speed based on distance
            float multiplier = Mathf.Lerp(minSpeedMultiplier, maxSpeedMultiplier, lerpSpeedPercentage);
            targetSpeed = baseSpeed * multiplier;
        }

        // Smooth transition to new speed
        MoveSpeed = Mathf.Lerp(MoveSpeed, targetSpeed, lerpCurrentSpeedTime); // Adjust 0.1f for a smoother or faster transition
    }


    private void MoveCharacter()
    {
        if (!IsOwner) return;

        if (targetPosition.HasValue)
        {
            // Reduce calculations by updating only at intervals
            float distanceToTarget = Vector3.Distance(agent.transform.position, targetPosition.Value);
            if (distanceToTarget >= minUpdateDistance)
            {
                float updateInterval = Mathf.Lerp(minUpdateInterval, maxUpdateInterval, Mathf.InverseLerp(minUpdateDistance, maxUpdateDistance, distanceToTarget));

                // Update path at the calculated interval
                if (Time.time - lastPathUpdateTime >= updateInterval)
                {
                    agent.SetDestination(targetPosition.Value);
                    lastPathUpdateTime = Time.time;
                }
            }
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();

        hasPath = agent.hasPath;
        isPending = agent.pathPending;
        remainingDistance = agent.remainingDistance;

        // Reduce calculations by only updating stats sometimes
        if (IsServer && agent.hasPath && !agent.pathPending)
        {
            UpdateMoveStats();
        }
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateMoveStats();
        }
    }
}
