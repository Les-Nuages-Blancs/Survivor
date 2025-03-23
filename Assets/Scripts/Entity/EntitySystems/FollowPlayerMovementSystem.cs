using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayerMovementSystem : NetworkBehaviour
{
    [SerializeField] private MovementSystem movementSystem;
    [SerializeField] private Animator animator;

    private static readonly int IsWalking = Animator.StringToHash("isWalking");



    void Update()
    {
        if (!IsServer) return;

        // TODO
        // need to calculate the closest one 
        // need to refresh transform not each frame

        if (Player.playerList.Count > 0)
        {
            Transform closestPlayerTransform = FindClosestPlayer();

            if (closestPlayerTransform != null)
            {
                movementSystem.targetPosition = closestPlayerTransform.position;
            }
        }

        //animator.SetBool(IsWalking, vertical != 0 || horizontal != 0);
    }
    private Transform FindClosestPlayer()
    {
        Transform closestTransform = null;
        float closestDistance = float.MaxValue;

        foreach (Player player in Player.playerList)
        {
            GameObject playerGo = player.gameObject;

            float distance = Vector3.Distance(transform.position, playerGo.transform.position);

            if (distance < closestDistance && !playerGo.GetComponent<PlayerDeathSystem>().IsDead)
            {
                closestDistance = distance;
                closestTransform = playerGo.transform;
            }
        }

        return closestTransform;
    }
}
