using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MovementInputSystem : NetworkBehaviour
{
    [SerializeField] private SimpleMovementSystem movementSystem;
    [SerializeField] private Animator characterAnimator;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    private float vertical;
    private float horizontal;

    void Update()
    {
        if (!IsOwner) return;

        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector3 currentPosition = movementSystem.transform.position;
        movementSystem.targetPosition = currentPosition + new Vector3(horizontal, 0, vertical);

        characterAnimator.SetBool(IsWalking, vertical != 0 || horizontal != 0);
    }
}
