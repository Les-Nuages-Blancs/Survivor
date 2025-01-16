using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class MovementSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] private NetworkVariable<float> moveSpeed = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] public UnityEvent onMoveSpeedChange;

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

    public Vector3? targetPosition = null;

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

    public void UpdateMoveStats()
    {
        if (!IsServer) return;
        float newValue = statsLevelSystem.BaseStatistiques.MoveSpeedMultiplier;
        moveSpeed.Value = newValue;
        agent.speed = newValue;
    }

    private void MoveCharacter()
    {
        if (!IsOwner) return;
        
        if (targetPosition.HasValue)
        {
            agent.SetDestination(targetPosition.Value);
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateMoveStats();
        }
    }
}
