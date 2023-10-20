using System.Linq;
using Systems;
using UnityEngine;

public class EnemyAIController : PausableCharacter
{
    private enum AIState
    {
        moving,
        pausing,
        attacking,
        reset
    }

    [SerializeField] private EnemyAIDataSO enemyAIData;

    [SerializeField] private Transform eyeTransform;

    [SerializeField] private Transform footTransform;

    [SerializeField] private Transform groundDetector;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private FieldOfView fieldOfView;

    private AIState _aiState;

    private float _pausingTimer = 0f;

    private float _currentDirection = 1f;

    private float _distanceToObstacle = 0.5f;

    private float _distanceToGround = -0.5f;

    private float _moveSpeed;

    private Transform _spawnLocation;

    private void Start()
    {
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
            case EnemyType.flying:
                _moveSpeed = 0;
                _aiState = AIState.pausing;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!_active) return;
        switch (enemyAIData.enemyType)
        {
            case EnemyType.chase:
                EnemyChaseMove();
                break;
            case EnemyType.sentry:
                EnemySentryMove();
                break;
            case EnemyType.flying:
                EnemyFlyingMove();
                break;
        }
    }

    private void EnemyFlyingMove()
    {
        switch (_aiState)
        {
            case AIState.pausing:
                _pausingTimer += Time.deltaTime;
                if (_pausingTimer >= enemyAIData.waitToMove)
                {
                    _currentDirection = -1 * _currentDirection;
                    _pausingTimer = 0;
                    _aiState = AIState.reset;
                }
                //animator.SetBool("Run", false);
                break;
            case AIState.attacking:
                if (!CanSeePlayer())
                {
                    print("Not see player");
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    _aiState = AIState.moving;
                    return;
                }
                print("see player");
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Fly();
                if (!CanFly())
                {
                    print("can't fly towards player, go to pause");
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                break;
            case AIState.reset:
                _pausingTimer += Time.deltaTime;
                if (_pausingTimer >= enemyAIData.waitToMove)
                {
                    _currentDirection = -1 * _currentDirection;
                    _pausingTimer = 0;
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", false);
                break;
        }
    }

    private void EnemySentryMove()
    {
        switch (_aiState)
        {
            case AIState.moving:
                //Drops enemy onto ground
                if (!IsGrounded(footTransform))
                {
                    ApplyGravity();
                    return;
                }
                //move until get to obstacle OR see player
                Move();
                if (!CanMove())
                {
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                if (CanSeePlayer())
                {
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
                if (!CanSeePlayer())
                {
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    _aiState = AIState.moving;
                    return;
                }
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Move();
                if (!CanMove())
                {
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                break;
        }
    }

    private void EnemyChaseMove()
    {
        switch (_aiState)
        {
            case AIState.pausing:
                if (CanSeePlayer())
                {
                    _aiState = AIState.attacking;
                }
                //animator.SetBool("Run", false);
                break;
            case AIState.attacking:
                if (!CanSeePlayer())
                {
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    _aiState = AIState.pausing;
                    return;
                }
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Move();
                if (!CanMove())
                {
                    _aiState = AIState.pausing;
                }
                //animator.SetBool("Run", true);
                break;
        }
    }

    private Vector3 GetDirectionToPlayer()
    {
        if (fieldOfView.visibleTargets.First() == null) { return transform.position; } //exit if no player is seen
        Transform target = fieldOfView.visibleTargets.First().transform; //we sent the first target, which will be the player
        return (target.position - transform.position).normalized;
    }

    private Vector3 Fly()
    {
        Vector3 directionTowardsTarget = GetDirectionToPlayer();
        transform.position = Vector2.MoveTowards(transform.position, directionTowardsTarget, _moveSpeed * Time.deltaTime);
        return directionTowardsTarget;
    }

    private void Move()
    {
        Vector2 forward = transform.right * _currentDirection;
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + forward * enemyAIData.detectionRange;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
        FaceTowardsMovement();
    }

    private bool CanFly()
    {
        //can only move if no obstacle directly in front of enemy
        return !IsNearObstacleInFront(GetDirectionToPlayer());
    }

    private bool CanMove()
    {
        //can only move if no obstacle directly in front of enemy AND ground to walk on
        Vector2 direction = eyeTransform.right * _currentDirection;
        return !IsNearObstacleInFront(direction) && IsNearGroundInFront();
    }

    private bool IsNearObstacleInFront(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, direction, _distanceToObstacle, obstacleLayer);
        Debug.DrawRay(eyeTransform.position, direction, Color.red, 1f);
        return hit.collider != null && IsNotThisEnemy(hit);
    }

    private bool IsNearGroundInFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundDetector.position, Vector2.down, 1f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down, Color.yellow, 1f);
        return hit.collider != null && IsNotThisEnemy(hit);
    }

    private bool IsGrounded(Transform fromLocation)
    {
        RaycastHit2D hit = Physics2D.Raycast(fromLocation.position, Vector2.down, _distanceToGround, groundLayer);
        Debug.DrawRay(fromLocation.position, Vector2.down, Color.blue, 1f);
        return hit.collider != null && IsNotThisEnemy(hit);
    }

    private void ApplyGravity()
    {
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + Vector2.down * enemyAIData.detectionRange;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
    }

    private bool IsTargetInRange(GameObject target, float range)
    {
        var distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        return distanceToTarget <= range;
    }

    private bool CanSeePlayer()
    {
        if (fieldOfView.visibleTargets.Count > 0)
        {
            return true;
        }

        return false;
    }

    private bool IsNotThisEnemy(RaycastHit2D hit)
    {
        return hit.transform.gameObject != gameObject;
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
