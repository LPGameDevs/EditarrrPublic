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

    public const string COLLISION_TRIGGER_NAME = "Collision";

    private void OnEnable()
    {
        _aiController.OnPlayerCollision += OnCollision;
        _aiController.OnStateChanged += ChangeAnimationState;
    }

    private void OnDisable()
    {
        _aiController.OnPlayerCollision -= OnCollision;
        _aiController.OnStateChanged -= ChangeAnimationState;
    }

    private void ChangeAnimationState(EnemyAIController.AIState newState)
    {
        _characterAnimator.SetInteger("StateEnum", (int)newState);
        _characterAnimator.ResetTrigger(COLLISION_TRIGGER_NAME);
    }

    private void OnCollision()
    {
        _characterAnimator.SetTrigger(COLLISION_TRIGGER_NAME);
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
