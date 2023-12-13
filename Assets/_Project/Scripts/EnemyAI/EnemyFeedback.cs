using MoreMountains.Feedbacks;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]

public class EnemyFeedback : MonoBehaviour
{
    [SerializeField] EnemyAIController _aiController;
    [SerializeField] Animator _dialogueAnimator;
    [Space(15)]
    [SerializeField] MMFeedbacks _move, _attack, _altAttack, alert, confusion;
    [Space(15)]
    [SerializeField] AudioSource _generalAudioSource;
    [SerializeField] AudioSource _moveAudioSource;
    [Range(0, 2)][SerializeField] float _minMovePitch, _maxMovePitch, _minMoveVolume, _maxMoveVolume;

    bool _isAttacking;

    private void OnMove(bool movingOnGround)
    {
        //if (isMoving)
        //    _move.PlayFeedbacks();
        //else
        //    _move.StopFeedbacks();

        if (movingOnGround)
        {
            if (!_moveAudioSource.isPlaying)
            {
                _moveAudioSource.volume = Random.Range(_minMoveVolume, _maxMoveVolume);
                _moveAudioSource.pitch = Random.Range(_minMovePitch, _maxMovePitch);
                _moveAudioSource.Play();
            }

            _move.PlayFeedbacks();
        }
        else
        {
            if (_moveAudioSource.isPlaying)
                _moveAudioSource.Stop();

            _move.StopFeedbacks();
        }
    }

    private void OnPlayerCollision()
    {
        if(_isAttacking && _altAttack != null)
            _altAttack.PlayFeedbacks();
        else
            _attack.PlayFeedbacks();
    }

    private void OnPlayerSpotted()
    {
        alert.PlayFeedbacks();
        _dialogueAnimator.SetTrigger("Exclamation");
    }

    private void OnLostSightOfPlayer()
    {
        confusion.PlayFeedbacks();
        _dialogueAnimator.SetTrigger("Confusion");
    }

    public void OnAttackStateChanged(bool isAttacking)
    {
        if (_altAttack == null)
            return;

        _isAttacking = isAttacking;
    }

    public void PlayAudioClip(AudioClip clip)
    {
        _generalAudioSource.PlayOneShot(clip);
    }

    private void OnEnable()
    {
        _aiController.OnMove += OnMove;
        _aiController.OnPlayerCollision += OnPlayerCollision;
        _aiController.OnAttackStateChanged += OnAttackStateChanged;
        _aiController.OnPlayerSpotted += OnPlayerSpotted;
        _aiController.OnLostSightOfPlayer += OnLostSightOfPlayer;

    }

    private void OnDisable()
    {
        _aiController.OnMove -= OnMove;
        _aiController.OnPlayerCollision -= OnPlayerCollision;
        _aiController.OnAttackStateChanged -= OnAttackStateChanged;
        _aiController.OnPlayerSpotted -= OnPlayerSpotted;
        _aiController.OnLostSightOfPlayer -= OnLostSightOfPlayer;
    }

}
