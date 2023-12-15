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

    #region fields
    public event Action<AIState> OnStateChanged;
    public event Action<bool> OnMove;
    public event Action<bool> OnAttackStateChanged;
    public event Action OnPlayerCollision;
    public event Action OnPlayerSpotted;
    public event Action OnLostSightOfPlayer;

    [SerializeField] private EnemyAIDataSO enemyAIData;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private Transform footTransform;
    [SerializeField] private Transform groundDetector;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask otherEnemyLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private FieldOfView fieldOfView;
    [SerializeField] private SpriteRenderer dialoguePopupRenderer;
    [SerializeField] private Hazard _collisionHazard;
    [SerializeField] private Collider2D _attackRangeCollider;

    private AIState _aiState;
    private float _timer = 0f;
    private float _distanceToObstacle = 1f;
    private float _distanceToGround = -0.5f;
    private float _moveSpeed;
    private Vector3 _spawnLocation;
    private bool _playerInSight;
    #endregion

    private void Start()
    {
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

    private void Update()
    {
        dialoguePopupRenderer.flipX = transform.localScale.x == -1 ? true : false;
    }

    private void EnemyAnticipator()
    {
        // Debug.Log("Clawdia state: " + _aiState.ToString());
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
                    _timer = 0;
                    _moveSpeed = 0;
                    TurnAround();
                    ChangeActiveState(AIState.moving);
                }
                break;

            case AIState.moving:
                _timer += Time.deltaTime;
                if (CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.alerting);
                    return;
                }

                if (!CanMove())
                {
                    _timer = 0;
                    ChangeActiveState(AIState.pausing);
                    return;
                }

                if (_timer >= enemyAIData.pauseTime * 1.5f)
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.idle);
                    return;
                }
                Move(enemyAIData.normalMoveSpeed, GetCurrentDirection());
                break;

            case AIState.pausing:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.pauseTime * 0.5f)
                {
                    _timer = 0;
                    if (!CanMove())
                        TurnAround();

                    ChangeActiveState(AIState.moving);
                }
                break;

            case AIState.alerting:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    if (_attackRangeCollider.IsTouchingLayers(playerLayer))
                    {
                        _timer = 0;
                        ChangeActiveState(AIState.attacking);
                        OnAttackStateChanged(true);
                    }
                }
                if(!CanSeePlayer())
                {
                    _timer = 0;
                    ChangeActiveState(AIState.pausing);
                    return;
                }
                break;

            case AIState.attacking:
                _timer += Time.deltaTime;
                if (_timer >= enemyAIData.alertTime)
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    ChangeActiveState(AIState.pausing);
                    OnAttackStateChanged(false);
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
                    fieldOfView.AdjustFOV(-1f, 360f);
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
                if (_timer >= enemyAIData.alertTime || !CanSeePlayer())
                {
                    ChangeActiveState(AIState.attacking);
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.attacking:
                if (!CanFly() || !CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    fieldOfView.RevertFOV();
                    ChangeActiveState(AIState.reseting);
                    return;
                }
                Fly(enemyAIData.sawPlayerMoveSpeed, GetPlayerPosition());
                break;

            case AIState.reseting:
                _timer += Time.deltaTime;
                if (CanSeePlayer())
                {
                    _timer = 0;
                    _moveSpeed = 0;
                    fieldOfView.AdjustFOV(-1f, 360f);
                    ChangeActiveState(AIState.attacking);
                    return;
                }
                if (_timer >= enemyAIData.pauseTime)
                {
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
                    _timer = 0;
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
                    OnAttackStateChanged(true);
                    _timer = 0;
                    _moveSpeed = 0;
                }
                break;

            case AIState.attacking:
                _timer += Time.deltaTime;

                if (!CanSeePlayer())
                {
                    if (_timer >= enemyAIData.alertTime * 3f)
                    {
                        _timer = 0;
                        _moveSpeed = 0;
                        ChangeActiveState(AIState.pausing);
                        OnAttackStateChanged(false);
                        return;
                    }
                }

                if (!CanMove())
                {
                    _moveSpeed = 0;
                    if (_timer >= enemyAIData.pauseTime)
                    {
                        _timer = 0;
                        _moveSpeed = 0;
                        ChangeActiveState(AIState.pausing);
                        OnAttackStateChanged?.Invoke(false);
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
        FaceTowardsTarget(targetPosition);
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
        return hit.collider != null && IsNotThisEnemy(hit);
    }

    private bool IsNearObstacleInFront(Vector3 direction, float distanceToObstacle)
    {
        RaycastHit2D hit = Physics2D.Raycast(eyeTransform.position, direction, distanceToObstacle, obstacleLayer);
        Debug.DrawRay(eyeTransform.position, direction, Color.black, 0.1f);
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
            if(!_playerInSight)
            {
                OnPlayerSpotted?.Invoke();
                _playerInSight = true;
            }
            return true;
        }

        if (_playerInSight)
        {
            OnLostSightOfPlayer?.Invoke();
            _playerInSight = false;
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
        if (directionX.Sign() != transform.localScale.x.Sign())
            _moveSpeed *= 0.5f;

        return directionX.Sign();
    }

    private void FaceTowardsTarget(Vector2 target)
    {
        Vector3 newScale = new Vector3(GetDirectionTowards(target), 1, 1);
        // Debug.Log(newScale);

        transform.localScale = newScale.x != 0 ? newScale : transform.localScale;
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

    private void OnCollision(Transform playerTransform)
    {
        _moveSpeed = 0f;
        FaceTowardsTarget(playerTransform.position);
        OnPlayerCollision?.Invoke();
    }

    internal override void OnEnable()
    {
        base.OnEnable();
        _collisionHazard.OnCollision += OnCollision;
    }

    internal override void OnDisable()
    {
        base.OnDisable();
        _collisionHazard.OnCollision -= OnCollision;
    }
}
