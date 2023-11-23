using Player;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAIController : PausableCharacter
{
    public enum AIState
    {
        idle,
        pausing,
        moving,
        alerting,
        attacking,
        reseting
    }

    public event Action<AIState> OnStateChanged;
    public event Action<bool> OnMove;

    [SerializeField] private EnemyAIDataSO enemyAIData;

    [SerializeField] private Transform eyeTransform;

    [SerializeField] private Transform footTransform;

    [SerializeField] private Transform groundDetector;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private LayerMask otherEnemyLayer;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private FieldOfView fieldOfView;

    [SerializeField] private Hazard hazard;

    private AIState _aiState;

    private float _timer = 0f;

    private float _distanceToObstacle = 1f;

    private float _distanceToGround = -0.5f;

    private float _moveSpeed;

    private Vector3 _spawnLocation;

    private float _originalFOVRadius;

    private void Start()
    {
        _originalFOVRadius = fieldOfView.viewRadius;
        _spawnLocation = transform.position;
        switch (enemyAIData.enemyType)
        {
            case EnemyType.dashAttack:
                _moveSpeed = enemyAIData.normalMoveSpeed;
                ChangeActiveState(AIState.pausing);
                break;

            case EnemyType.sentry:
                _moveSpeed = enemyAIData.normalMoveSpeed;
                ChangeActiveState(AIState.moving);
                break;

            case EnemyType.lunging:
                _moveSpeed = enemyAIData.normalMoveSpeed;
                ChangeActiveState(AIState.moving);
                break;

            case EnemyType.flying:
                _moveSpeed = 0;
                ChangeActiveState(AIState.idle);
                break;

            case EnemyType.anticipator:
                _moveSpeed = 0;
                ChangeActiveState(AIState.idle);
                break;
        }
    }

    private void FixedUpdate()
    {
        if (!_active) return;

        if (!IsGrounded(footTransform))
            ApplyGravity();

        switch (enemyAIData.enemyType)
        {
            case EnemyType.dashAttack:
                EnemyDashAttack();
                break;

            case EnemyType.sentry:
                EnemySentry();
                break;

            case EnemyType.lunging:
                EnemyLunging();
                break;

            case EnemyType.flying:
                EnemyFlying();
                break;

            case EnemyType.anticipator:
                EnemyAnticipator();
                break;
        }
    }

    private void EnemyAnticipator()
    {
        if (!IsGrounded(footTransform))
        {
            _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
            ApplyGravity();
            return;
        }

        switch (_aiState)
        {
            case AIState.idle:
                if (CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.alerting);
                    return;
                }
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.moving:
                if (!CanMove())
                {
                    ChangeActiveState(AIState.pausing);
                }
                if (CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.alerting);
                    return;
                }

                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.idle);
                    return;
                }
                Move(enemyAIData.normalMoveSpeed, GetCurrentDirection());
                break;

            case AIState.pausing:
                fieldOfView.viewRadius = _originalFOVRadius;
                //Drops enemy onto ground
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    if(!CanMove())
                        TurnAround();

                    _timer = 0;
                    ChangeActiveState(AIState.moving);
                }
                break;

            case AIState.alerting:
                fieldOfView.viewRadius = 2;
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    if(CanSeePlayer())
                    {
                        _timer = 0;
                        ChangeActiveState(AIState.attacking);
                    }
                }
                if(_timer >= enemyAIData.pauseTime)
                {
                    _timer = 0;
                    ChangeActiveState(AIState.pausing);
                    return;
                }
                break;

            case AIState.attacking:
                hazard.AdjustDamage(10);
                hazard.AdjustKnockback(60);
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.pausing);
                    hazard.AdjustDamage(5);
                    hazard.AdjustKnockback(10);
                    return;
                }
                break;
        }
    }

    private void EnemyFlying()
    {
        switch (_aiState)
        {
            case AIState.idle:
                if (CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.alerting);
                    return;
                }
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;
            case AIState.alerting:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    ChangeActiveState(AIState.attacking);
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.attacking:
                if (!CanFly() || !CanSeePlayer())
                {
                    print("Can't fly towards player OR can't see player");
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.reseting);
                    return;
                }
                print("see player");
                Fly(enemyAIData.sawPlayerMoveSpeed, GetPlayerPosition());
                break;

            case AIState.reseting:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    if (CanSeePlayer())
                    {
                        _timer = 0;
                        _moveSpeed = 0;
                        ChangeActiveState(AIState.attacking);
                        return;
                    }
                    Fly(enemyAIData.normalMoveSpeed, _spawnLocation);
                    if (Vector3.Distance(transform.position, _spawnLocation) <= 0.2f)
                    {
                        _timer = 0;
                        _moveSpeed = 0;
                        ChangeActiveState(AIState.idle);
                    }
                }
                break;
        }
    }

    private void EnemySentry()
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
                Move(enemyAIData.normalMoveSpeed, GetCurrentDirection());
                if (!CanMove())
                {
                    ChangeActiveState(AIState.pausing);
                }
                if (CanSeePlayer())
                {
                    ChangeActiveState(AIState.alerting);
                }
                break;

            case AIState.alerting:
                if (CanSeePlayer())
                {
                    ChangeActiveState(AIState.attacking);
                }
                break;

            case AIState.pausing:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    ChangeActiveState(AIState.moving);
                }
                break;

            case AIState.attacking:
                if (!CanSeePlayer())
                {
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    ChangeActiveState(AIState.moving);
                    return;
                }
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Move(enemyAIData.sawPlayerMoveSpeed, GetCurrentDirection());
                if (!CanMove())
                {
                    ChangeActiveState(AIState.pausing);
                }
                break;
        }
    }

    private void EnemyLunging()
    {
        print(_aiState);
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
                Move(enemyAIData.normalMoveSpeed, GetCurrentDirection());
                if (!CanMove())
                {
                    ChangeActiveState(AIState.pausing);
                }
                if (CanSeePlayer())
                {
                    ChangeActiveState(AIState.alerting);
                }
                break;

            case AIState.alerting:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    if (CanSeePlayer())
                    {
                        ChangeActiveState(AIState.attacking);
                    }
                }
                break;

            case AIState.pausing:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    ChangeActiveState(AIState.moving);
                }
                break;

            case AIState.attacking:
                if (!CanSeePlayer())
                {
                    _moveSpeed = enemyAIData.normalMoveSpeed;
                    ChangeActiveState(AIState.moving);
                    return;
                }
                _moveSpeed = enemyAIData.sawPlayerMoveSpeed;
                Move(enemyAIData.sawPlayerMoveSpeed, GetCurrentDirection());
                if (!CanMove())
                {
                    ChangeActiveState(AIState.pausing);
                }
                break;
        }
    }

    private void EnemyDashAttack()
    {
        switch (_aiState)
        {
            case AIState.pausing:
                //Drops enemy onto ground
                if (!IsGrounded(footTransform))
                {
                    ApplyGravity();
                    return;
                }
                if (CanSeePlayer())
                {
                    ChangeActiveState(AIState.alerting);
                }
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime)
                {
                    TurnAround();
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.alerting:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    ChangeActiveState(AIState.attacking);
                    hazard.AdjustDamage(10);
                    hazard.AdjustKnockback(40);
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.attacking:
                if (!CanSeePlayer())
                {
                    _timer += Time.deltaTime;
                    if (_timer >= enemyAIData.pauseTime)
                    {
                        _timer = 0;
                        _moveSpeed = 0;
                        print("Can't see player, pausing");
                        ChangeActiveState(AIState.pausing);
                        hazard.AdjustDamage(5);
                        hazard.AdjustKnockback(10);
                        return;
                    }
                }

                if (!CanMove())
                {
                    print("Can't move, pausing");
                    _moveSpeed = 0;
                    _timer += Time.deltaTime;
                    if (_timer >= enemyAIData.pauseTime)
                    {
                        TurnAround();
                        _timer = 0;
                        print("Can't move, pausing");
                        ChangeActiveState(AIState.pausing);
                        hazard.AdjustDamage(5);
                        hazard.AdjustKnockback(10);
                        return;
                    }
                }
                Move(enemyAIData.sawPlayerMoveSpeed, GetCurrentDirection());
                break;
        }
    }

    private Vector3 GetDirectionToPlayer()
    {
        if (fieldOfView.visibleTargets.Count == 0) { return transform.position; } //exit if no player is seen
        Transform target = fieldOfView.visibleTargets.First().transform; //we sent the first target, which will be the player
        return (target.position - transform.position).normalized;
    }

    private Vector3 GetPlayerPosition()
    {
        if (fieldOfView.visibleTargets.Count == 0) { return transform.position; } //exit if no player is seen
        return fieldOfView.visibleTargets.First().transform.position; //we sent the first target, which will be the player
    }

    private void Fly(float finalSpeed, Vector2 targetPosition)
    {
        _moveSpeed = Mathf.Lerp(_moveSpeed, finalSpeed, Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
        FaceTowardsMovement(targetPosition);
    }

    private void Move(float finalSpeed, float direction)
    {
        _moveSpeed = Mathf.Lerp(_moveSpeed, finalSpeed, Time.deltaTime);
        Vector2 forward = -1 * transform.right * direction; //have to reverse this because the player starts facing the left direction
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + forward * _distanceToGround;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
        OnMove?.Invoke(CanMove() && IsGrounded(footTransform));
        //FaceTowardsMovement(targetPosition);
    }

    private bool CanFly()
    {
        //can only move if no obstacle directly in front of enemy
        return !IsNearObstacleInFront(GetDirectionToPlayer(), _distanceToObstacle);
    }

    private bool CanMove()
    {
        //can only move if no obstacle directly in front of enemy AND ground to walk on
        Vector2 direction = eyeTransform.right * GetCurrentDirection();
        return !IsNearObstacleInFront(direction, _distanceToObstacle) && !IsNearEnemyInFront(direction, _distanceToObstacle) && IsNearGroundInFront();
    }

    private bool IsNearEnemyInFront(Vector3 direction, float distanceToObstacle)
    {
        RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, direction, distanceToObstacle, otherEnemyLayer);
        //Debug.DrawRay(eyeTransform.position, direction, Color.red, 0.1f);
        if (hit.collider != null && IsNotThisEnemy(hit)) print(hit.collider);
        return hit.collider != null && IsNotThisEnemy(hit);
    }

    private bool IsNearObstacleInFront(Vector3 direction, float distanceToObstacle)
    {
        RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, direction, distanceToObstacle, obstacleLayer);
        Debug.DrawRay(eyeTransform.position, direction, Color.black, 0.1f);
        if (hit.collider != null) print(hit.collider);
        return hit.collider != null;
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
        if (enemyAIData.enemyType == EnemyType.flying)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = currentPosition + Vector2.down * enemyAIData.detectionRange;
        transform.position = Vector2.MoveTowards(currentPosition, targetPosition, _moveSpeed * Time.deltaTime);
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

    private float GetDirectionTowards(Vector2 targetPosition)
    {
        float directionX = transform.position.x - targetPosition.x;
        return directionX;
    }

    private void FaceTowardsMovement(Vector2 directionTowardsTarget)
    {
        if (GetDirectionTowards(directionTowardsTarget) < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void TurnAround()
    {
        float x = -1 * transform.localScale.x;
        transform.localScale = new Vector3(x, 1f, 1f);
    }

    private float GetCurrentDirection()
    {
        // 1 is right, -1 is left
        float x = transform.localScale.x;
        return -1 * x;
    }

    private void ChangeActiveState(AIState newState)
    {
        _aiState = newState;
        OnStateChanged?.Invoke(_aiState);
    }
}
