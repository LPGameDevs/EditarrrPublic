using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private enum AIState
    {
        moving,
        pausing,
        attacking
    }

    [SerializeField] private EnemyAIDataSO enemyAIData;

    [SerializeField] private Transform eyeTransform;

    [SerializeField] private Transform footTransform;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private LayerMask obstacleLayer;

    private AIState _aiState;

    private float _pausingTimer = 0f;

    private float _currentDirection = 1f;

    private float _distanceToObstacle = 0.5f;

    private float _moveSpeed;

    private Collider2D enemyCollider;

    private void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        switch (enemyAIData.enemyType)
        {
            case EnemyType.chase:
                _moveSpeed = 0;
                _aiState = AIState.pausing;
                break;
            case EnemyType.sentry:
                _moveSpeed = enemyAIData.normalMoveSpeed;
                _aiState = AIState.moving;
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (enemyAIData.enemyType)
        {
            case EnemyType.chase:
                EnemyChaseMove();
                break;
            case EnemyType.sentry:
                EnemySentryMove();
                break;
        }
    }

    private void EnemySentryMove()
    {
        switch (_aiState)
        {
            case AIState.moving:
                //move until get to obstacle OR see player
                Move();
                if (!CanMove())
                {
                    print("can't move, go to pause");
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                if (IsNearPlayer())
                {
                    print("see player, attack");
                    _aiState = AIState.attacking;
                }
                break;
            case AIState.pausing:
                _pausingTimer += Time.deltaTime;
                if (_pausingTimer >= enemyAIData.waitToMove)
                {
                    _currentDirection = -1 * _currentDirection;
                    _pausingTimer = 0;
                    _aiState = AIState.moving;
                }
                //animator.SetBool("Run", false);
                break;
            case AIState.attacking:
                if (!IsNearPlayer())
                {
                    print("Not see player");
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    _aiState = AIState.moving;
                    return;
                }
                print("see player");
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Move();
                if (!CanMove())
                {
                    print("can't move, go to pause");
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                break;
        }
    }

    private void EnemyChaseMove()
    {
        throw new NotImplementedException();
    }

    private void Move()
    {
        Vector2 forward = transform.right * _currentDirection;
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + forward * enemyAIData.detectionRange;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
        FaceTowardsMovement();
    }

    private bool CanMove()
    {
        //can only move if no obstacle directly in front of enemy AND ground to walk on
        if (!IsNearObstacle(eyeTransform))
        {
            print("Nothing in front of enemy");
        }
        if (IsNearObstacle(footTransform))
        {
            print("Ground near enemy's foot");
        }

        return !IsNearObstacle(eyeTransform) && IsNearObstacle(footTransform);
    }

    private bool IsNearObstacle(Transform fromLocation)
    {
        Vector2 direction = fromLocation.right * _currentDirection;
        RaycastHit2D hit = Physics2D.Raycast(fromLocation.position, direction, _distanceToObstacle, obstacleLayer);
        return hit.collider != null && enemyCollider != hit.collider;
    }

    private bool IsTargetInRange(GameObject target, float range)
    {
        var distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        return distanceToTarget <= range;
    }

    private bool IsNearPlayer()
    {
        Vector2 direction = transform.right * _currentDirection;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, enemyAIData.detectionRange);

        //0 is the enemy, 1 needs to be the player for the enemy to see him
        foreach (var hit in hits)
        {
            if (playerLayer == (playerLayer | (1 << hit.transform.gameObject.layer))) { return true; }
            if (obstacleLayer == (obstacleLayer | (1 << hit.transform.gameObject.layer))) { return false; }
        }

        return false;
    }

    private void FaceTowardsMovement()
    {
        if (_currentDirection > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;

        }
    }
}
