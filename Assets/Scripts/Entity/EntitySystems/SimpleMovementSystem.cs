using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class SimpleMovementSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;

    [SerializeField] private NetworkVariable<float> moveSpeed = new NetworkVariable<float>(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] public UnityEvent onMoveSpeedChange;

    public float MoveSpeed
    {
        get => moveSpeed.Value;
        set
        {
            if (!Mathf.Approximately(moveSpeed.Value, value))
            {
                moveSpeed.Value = value;
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
        moveSpeed.Value = newValue;

        onMoveSpeedChange.Invoke();
    }

    public void UpdateMoveStats()
    {
        if (!IsServer) return;
        var newValue = statsLevelSystem.BaseStatistiques.MoveSpeedMultiplier;
        moveSpeed.Value = newValue;
    }

    private void MoveCharacter()
    {
        if (!IsOwner) return;

        if (!targetPosition.HasValue || Vector3.Distance(transform.position, targetPosition.Value) <= 0.1f) return;
        var currentPosition = transform.position;
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition.Value, MoveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
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
