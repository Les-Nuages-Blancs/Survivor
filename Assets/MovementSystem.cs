using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private StatistiquesLevelSystem statsLevelSystem;
    [SerializeField] private Animator characterAnimator;

    [SerializeField] private float moveSpeed;

    private float vertical;
    private float horizontal;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private void Start()
    {
        UpdateMoveStats();
    }

    public void UpdateMoveStats()
    {
        moveSpeed = statsLevelSystem.BaseStatistiques.MoveSpeedMultiplier;
    }

    private void MoveCharacter()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        characterAnimator.SetBool(IsWalking, vertical != 0 || horizontal != 0);
        currentPosition = transform.position;
        targetPosition = currentPosition + new Vector3(horizontal, 0, vertical);
        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.fixedDeltaTime);

    }

    private void RotateTowardsCursor()
    {
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
