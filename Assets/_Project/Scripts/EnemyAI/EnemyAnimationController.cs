using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] EnemyAIController _aiController;

    private void OnEnable()
    {
        _aiController.OnStateChanged += ChangeAnimationState;
    }

    private void OnDisable()
    {
        _aiController.OnStateChanged -= ChangeAnimationState;
    }

    private void ChangeAnimationState(EnemyAIController.AIState newState)
    {
        _animator.SetInteger("StateEnum", (int)newState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerController>(out PlayerController playerController))
            _animator.SetTrigger("Attack");
    }

    private void OnCollisionEnter2D(Collision2D collision) => OnTriggerEnter2D(collision.collider);
}
