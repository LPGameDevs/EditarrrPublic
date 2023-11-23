using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    public event Action OnAttack;

    [SerializeField] Animator _animator;
    [SerializeField] EnemyAIController _aiController;
    [SerializeField] GameObject attackParticlesPrefab;

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
        _animator.ResetTrigger("Attack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent<PlayerController>(out PlayerController playerController) || playerController.Health.IsInvincible())
            return;

        _animator.SetTrigger("Attack");
        OnAttack?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision) => OnTriggerEnter2D(collision.collider);

    public void SpawnAttackParticles(int spawnInWorldSpace)
    {

        GameObject particleInstance = (spawnInWorldSpace == 0 ?
                        Instantiate(attackParticlesPrefab, transform):
                        Instantiate(attackParticlesPrefab, transform.position, Quaternion.identity)); 

        Destroy(particleInstance, 1f);
    }
}
