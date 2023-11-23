using MoreMountains.Feedbacks;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]

public class EnemyFeedback : MonoBehaviour
{
    [SerializeField] EnemyAIController _aiController;
    [SerializeField] EnemyAnimationController _animationController;
    [SerializeField] MMFeedbacks _move, _attack;
    [Space(15)]
    [SerializeField] AudioSource _generalAudioSource;
    [SerializeField] AudioSource _moveAudioSource;
    [Range(0, 2)][SerializeField] float _minMovePitch, _maxMovePitch, _minMoveVolume, _maxMoveVolume;

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

        Debug.Log("OnMove");
    }

    private void OnAttack()
    {
        _attack.PlayFeedbacks();
    }

    private void OnEnable()
    {
        _aiController.OnMove += OnMove;
        _animationController.OnAttack += OnAttack;
    }

    private void OnDisable()
    {
        _aiController.OnMove -= OnMove;
        _animationController.OnAttack -= OnAttack;
    }
}
