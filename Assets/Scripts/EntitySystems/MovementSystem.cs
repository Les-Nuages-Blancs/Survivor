using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class MovementSystem : NetworkBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private Animator characterAnimator;

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
            }
        }
    }

    private float vertical;
    private float horizontal;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

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
        onMoveSpeedChange.Invoke();
    }

    public void UpdateMoveStats()
    {
        if (!IsServer) return;
        moveSpeed.Value = statsLevelSystem.BaseStatistiques.MoveSpeedMultiplier;
    }

    private void MoveCharacter()
    {
        if (!IsOwner) return;
        
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        characterAnimator.SetBool(IsWalking, vertical != 0 || horizontal != 0);
        currentPosition = transform.position;
        targetPosition = currentPosition + new Vector3(horizontal, 0, vertical);
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, MoveSpeed * Time.fixedDeltaTime);
    }

    private void RotateTowardsCursor()
    {
        if (!IsOwner) return;

        // Récupération de la position de la souris
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Détection de la position sur le sol (plan horizontal)
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
        {
            Vector3 lookAtPoint = hitInfo.point;
            lookAtPoint.y = transform.position.y; // Garde la rotation uniquement sur l'axe Y
            transform.LookAt(lookAtPoint);
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void Update()
    {
        RotateTowardsCursor();
    }

    private void OnValidate()
    {
        if (statsLevelSystem)
        {
            UpdateMoveStats();
        }
    }
}
