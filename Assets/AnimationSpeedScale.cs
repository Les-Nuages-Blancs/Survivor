using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedScale : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float speedScaleMultiplier = 1.0f;
    [SerializeField] private bool useLinearScale = false;
    [SerializeField] private MovementSystem movementSystem;

    public void ScaleAnimationSpeed()
    {
        animator.speed = speedScaleMultiplier * (useLinearScale ? 1.0f : movementSystem.MoveSpeed);
    }
}
