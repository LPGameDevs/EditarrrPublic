using MoreMountains.Feedbacks;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] Animator _characterAnimator;
    [SerializeField] EnemyAIController _aiController;
    [SerializeField] GameObject _attackParticlesPrefab;
    [SerializeField] EnemyFeedback _enemyFeedback;

    private void OnEnable()
    {
        _aiController.OnStateChanged += ChangeAnimationState;
        _aiController.OnPlayerCollision += OnAttack;
    }

    private void OnDisable()
    {
        _aiController.OnPlayerCollision -= OnAttack;
        _aiController.OnStateChanged -= ChangeAnimationState;
    }

    private void ChangeAnimationState(EnemyAIController.AIState newState)
    {
        _characterAnimator.SetInteger("StateEnum", (int)newState);
        _characterAnimator.ResetTrigger("Attack");
    }

    private void OnAttack()
    {
        _characterAnimator.SetTrigger("Attack");
    }

    #region Animation Events
    public void SpawnAttackParticles(int spawnInWorldSpace)
    {

        GameObject particleInstance = (spawnInWorldSpace == 0 ?
                        Instantiate(_attackParticlesPrefab, transform):
                        Instantiate(_attackParticlesPrefab, transform.position, Quaternion.identity)); 

        Destroy(particleInstance, 1f);
    }

    public void PlayAudioClip(AudioClip audioClip) => _enemyFeedback.PlayAudioClip(audioClip);
    #endregion
}
