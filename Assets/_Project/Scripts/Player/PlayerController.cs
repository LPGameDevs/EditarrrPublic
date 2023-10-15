using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Editarrr.Input;
using Gameplay.GUI;
using Systems;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Adapted from Tarodev's Ultimate 2D controller, found here: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller
    /// </summary>
    [RequireComponent(typeof(HealthSystem), typeof(PlayerForceReceiver))]
    public class PlayerController : PausableCharacter
    {
        // events
        public static event Action OnPlayerJumped;
        public static event Action OnPlayerStartedFalling;
        public static event Action<float> OnPlayerLanded;
        public static event Action<bool> OnPlayerMoved;

        private HealthSystem _health;
        private Animator _animator;
        private PlayerForceReceiver _forceReceiver;

        public bool IsMoving => _isMoving;

        [SerializeField] Rigidbody2D rigidBody;
        private Vector3 _velocity;
        private Vector3 _rawMovement;
        private Vector3 _lastPosition;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;

        void Awake()
        {
            _health = GetComponent<HealthSystem>();
            _animator = GetComponentInChildren<Animator>();
            _forceReceiver = GetComponent<PlayerForceReceiver>();
        }

        public override void Deactivate()
        {
            base.Deactivate();
            _velocity = Vector3.zero;
            _rawMovement = Vector3.zero;
            _currentHorizontalSpeed = 0f;
            _currentVerticalSpeed = 0f;
            _isMoving = false;
        }

        void Deactivate(object sender, System.EventArgs e) => Deactivate();

        void LockInput(bool applyLock) => _inputLocked = applyLock;

        void TakeDamage(object sender, HealthSystem.OnHealthChangedArgs healthArgs)
        {
            if (healthArgs.value <= 0)
                return;

            if (healthArgs.stunDuration > 0)
                StartCoroutine(CoroutineStun(healthArgs.stunDuration));
        }

        IEnumerator CoroutineStun(float stunDuration)
        {
            Debug.Log("Start Stun");
            LockInput(true);

            yield return new WaitForSeconds(stunDuration);

            Debug.Log("Stop Stun");
            LockInput(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                LockInput(!_inputLocked);


            if (!_active) return;
            // Calculate velocity
            _velocity = (transform.position - _lastPosition) / Time.deltaTime;
            _lastPosition = transform.position;

            GatherInput();
            RunCollisionChecks();

            CalculateWalk(); // Horizontal movement
            CalculateJumpApex(); // Affects fall speed, so calculate before gravity
            CalculateGravity(); // Vertical movement
            CalculateJump(); // Possibly overrides vertical

            MoveCharacter(); // Actually perform the axis movement
            HandleSpriteDirection();

            HandleAnimationVariables();
        }

        private void HandleAnimationVariables()
        {
            OnPlayerMoved(_collisions.down && _isMoving);

            _animator.SetFloat(VerticalVelocityAnim, _currentVerticalSpeed);
            _animator.SetBool(GroundedAnim, _collisions.down);

            // @todo Should we take collision checks into account here?
            _animator.SetBool(IsMovingAnim, _isMoving);
            _animator.SetBool(JumpStartedAnim, _jumpStartThisFrame);
        }


        #region Gather Input
        [field: SerializeField, Tooltip("Move input map")] private InputValue MoveInput { get; set; }
        [field: SerializeField, Tooltip("Jump input map")] private InputValue JumpInput { get; set; }
        private bool _isMoving;
        private bool _jumpStartThisFrame;
        private bool _jumpReleaseThisFrame;
        private float _movementValue;


        private void GatherInput()
        {
            _jumpStartThisFrame = _jumpReleaseThisFrame = false;
            _movementValue = _inputLocked ? 0f : MoveInput.Read<Vector2>().x;

            _isMoving = MoveInput.IsPressed && !_inputLocked;

            if (_inputLocked)
                return;


            if (JumpInput.WasPressed)
            {
                _jumpStartThisFrame = true;
                _lastJumpPressed = Time.time;
            }
            else if (JumpInput.WasReleased)
            {
                _jumpReleaseThisFrame = true;
            }
        }

        #endregion

        #region Collisions

        [Header("COLLISION")][SerializeField] private Bounds _characterBounds;
        [SerializeField, Tooltip("Layer(s) to interact with for neutral collisions")] private LayerMask _groundLayer;
        [SerializeField, Tooltip("Number of ray casts per bounding box side")] private int _detectorCount = 3;
        [SerializeField, Tooltip("Length of collision raycasts")] private float _detectionRayLength = 0.1f;
        [SerializeField, Range(0, 0.3f), Tooltip("Inset distance of raycasts")] private float _rayBuffer = 0.1f; // Prevents side detectors hitting the ground

        private Direction2D<RayRange> _rays = new Direction2D<RayRange>(new RayRange());
        private Direction2D<bool> _collisions = new Direction2D<bool>(false);

        private float _timeLeftGrounded;
        private float _timeInAir;

        // We use these raycast checks for pre-collision information
        private void RunCollisionChecks()
        {
            // Generate ray ranges.
            CalculateRayRanges();

            // Ground
            var groundedCheck = RunDetection(_rays.down);
            if (!groundedCheck) _timeInAir += Time.deltaTime;
            if (_collisions.down && !groundedCheck) _timeLeftGrounded = Time.time; // Only trigger when first leaving
            else if (!_collisions.down && groundedCheck)
            {
                _coyoteUsable = true; // Only trigger when first touching
                OnPlayerLanded?.Invoke(_currentVerticalSpeed);
                _timeInAir = 0f;
            }
            _collisions.down = groundedCheck;

            // The rest
            _collisions.up = RunDetection(_rays.up);
            _collisions.left = RunDetection(_rays.left);
            _collisions.right = RunDetection(_rays.right);

            bool RunDetection(RayRange range)
            {
                return EvaluateRayPositions(range).Any(point => Physics2D.Raycast(point, range.Dir, _detectionRayLength, _groundLayer));
            }
        }

        private void CalculateRayRanges()
        {
            float bottom = _characterBounds.min.y + transform.position.y;
            float top = _characterBounds.max.y + transform.position.y;
            float left = _characterBounds.min.x + transform.position.x;
            float right = _characterBounds.max.x + transform.position.x;

            _rays.down = new RayRange(left + _rayBuffer, bottom, right - _rayBuffer, bottom, Vector2.down);
            _rays.up = new RayRange(left + _rayBuffer, top, right - _rayBuffer, top, Vector2.up);
            _rays.left = new RayRange(left, bottom + _rayBuffer, left, top - _rayBuffer, Vector2.left);
            _rays.right = new RayRange(right, bottom + _rayBuffer, right, top - _rayBuffer, Vector2.right);
        }


        private IEnumerable<Vector2> EvaluateRayPositions(RayRange range)
        {
            for (var i = 0; i < _detectorCount; i++)
            {
                var t = (float)i / (_detectorCount - 1);
                yield return Vector2.Lerp(range.Start, range.End, t);
            }
        }

        private void OnDrawGizmos()
        {
            // Bounds
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center, _characterBounds.size);

            // Rays
            if (!Application.isPlaying)
            {
                CalculateRayRanges();
                Gizmos.color = Color.blue;
                foreach (var range in new List<RayRange> { _rays.up, _rays.right, _rays.down, _rays.left })
                {
                    foreach (var point in EvaluateRayPositions(range))
                    {
                        Gizmos.DrawRay(point, range.Dir * _detectionRayLength);
                    }
                }
            }

            if (!Application.isPlaying) return;

            // Draw the future position. Handy for visualizing gravity
            Gizmos.color = Color.red;
            var move = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed) * Time.deltaTime;
            Gizmos.DrawWireCube(transform.position + _characterBounds.center + move, _characterBounds.size);
        }

        #endregion

        #region Walk

        [Header("WALKING")][SerializeField] private float _acceleration = 90;
        [SerializeField, Tooltip("Maximum move speed")] private float _maxMoveSpeed = 13;
        [SerializeField] private float _deAcceleration = 60f;
        [SerializeField] private float _apexBonus = 2;

        private void CalculateWalk()
        {
            if (_isMoving)
            {
                // Set horizontal move speed
                _currentHorizontalSpeed += _movementValue * _acceleration * Time.deltaTime;

                // clamped by max frame movement
                _currentHorizontalSpeed = Mathf.Clamp(_currentHorizontalSpeed, -_maxMoveSpeed, _maxMoveSpeed);

                // Apply bonus at the apex of a jump
                var apexBonus = Mathf.Sign(_movementValue) * _apexBonus * _apexPoint;
                _currentHorizontalSpeed += apexBonus * Time.deltaTime;
            }
            else
            {
                // No input. Let's slow the character down
                _currentHorizontalSpeed = Mathf.MoveTowards(_currentHorizontalSpeed, 0, _deAcceleration * Time.deltaTime);
            }

            // Add external force to movement if one is being applied, pre-collision adjustment
            if (_forceReceiver.ForcedMove.HasValue)
            {
                _currentHorizontalSpeed = _currentHorizontalSpeed + _forceReceiver.ForcedMove.Value.x;

                if (_forceReceiver.ForcedMove.Value.x > 0 && _currentHorizontalSpeed > _forceReceiver.ForcedMove.Value.x)
                    _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
                else if (_forceReceiver.ForcedMove.Value.x < 0 && _currentHorizontalSpeed < -_forceReceiver.ForcedMove.Value.x)
                    _currentHorizontalSpeed = _forceReceiver.ForcedMove.Value.x;
            }

            if (_currentHorizontalSpeed > 0 && _collisions.right || _currentHorizontalSpeed < 0 && _collisions.left)
            {
                // Don't walk through walls
                _currentHorizontalSpeed = 0;
            }
        }

        #endregion

        #region Gravity

        [Header("GRAVITY")][SerializeField] private float _fallClamp = -40f;
        [SerializeField] private float _minFallSpeed = 80f;
        [SerializeField] private float _maxFallSpeed = 120f;
        private float _fallSpeed;

        private void CalculateGravity()
        {
            if (_collisions.down || _forceReceiver.ForcedMove != null)
            {
                // Move out of the ground
                if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
            }
            else
            {
                // Add downward force while ascending if we ended the jump early
                var fallSpeed = _endedJumpEarly && _currentVerticalSpeed > 0 ? _fallSpeed * _jumpEndEarlyGravityModifier : _fallSpeed;

                // Fall
                float bufferedVerticalSpeed = _currentVerticalSpeed;
                _currentVerticalSpeed -= fallSpeed * Time.deltaTime;

                // Clamp
                if (_currentVerticalSpeed < _fallClamp) _currentVerticalSpeed = _fallClamp;

                if (_currentVerticalSpeed < 0f && bufferedVerticalSpeed >= 0f)
                    OnPlayerStartedFalling?.Invoke();
            }
        }

        #endregion

        #region Jump

        [Header("JUMPING")][SerializeField] private float _jumpHeight = 30;
        [SerializeField] private float _jumpApexThreshold = 10f;
        [SerializeField] private float _coyoteTimeThreshold = 0.1f;
        [SerializeField] private float _jumpBuffer = 0.1f;
        [SerializeField] private float _jumpEndEarlyGravityModifier = 3;
        private bool _coyoteUsable;
        private bool _endedJumpEarly = true;
        private float _apexPoint; // Becomes 1 at the apex of a jump
        private float _lastJumpPressed;
        private bool CanUseCoyote => _coyoteUsable && !_collisions.down && _timeLeftGrounded + _coyoteTimeThreshold > Time.time;
        private bool HasBufferedJump => _collisions.down && _lastJumpPressed + _jumpBuffer > Time.time;

        private void CalculateJumpApex()
        {
            if (!_collisions.down)
            {
                // Gets stronger the closer to the top of the jump
                _apexPoint = Mathf.InverseLerp(_jumpApexThreshold, 0, Mathf.Abs(_velocity.y));
                _fallSpeed = Mathf.Lerp(_minFallSpeed, _maxFallSpeed, _apexPoint);
            }
            else
            {
                _apexPoint = 0;
            }
        }

        private void CalculateJump()
        {
            // Jump if: grounded or within coyote threshold || sufficient jump buffer
            if (_jumpStartThisFrame && CanUseCoyote || HasBufferedJump)
            {
                _currentVerticalSpeed = _jumpHeight;
                _endedJumpEarly = false;
                _coyoteUsable = false;
                _timeLeftGrounded = float.MinValue;
                OnPlayerJumped?.Invoke();
            }

            // End the jump early if button released
            if (!_collisions.down && _jumpReleaseThisFrame && !_endedJumpEarly && _velocity.y > 0)
            {
                // _currentVerticalSpeed = 0;
                _endedJumpEarly = true;
            }

            //Overwrite movement with external force if one is being applied, pre-collision adjustment
            if (_forceReceiver.ForcedMove.HasValue)
                _currentVerticalSpeed = _forceReceiver.ForcedMove.Value.y;

            if (_collisions.up)
            {
                if (_currentVerticalSpeed > 0) _currentVerticalSpeed = 0;
            }
        }

        #endregion

        #region Move

        [Header("MOVE")]
        [SerializeField, Tooltip("Raising this value increases collision accuracy at the cost of performance.")]
        private int _freeColliderIterations = 10;

        private static readonly int IsMovingAnim = Animator.StringToHash("IsMoving");
        private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
        private static readonly int JumpStartedAnim = Animator.StringToHash("JumpStarted");
        private static readonly int VerticalVelocityAnim = Animator.StringToHash("VerticalVelocity");

        // We cast our bounds before moving to avoid future collisions
        private void MoveCharacter()
        {
            var pos = transform.position + _characterBounds.center;
            _rawMovement = new Vector3(_currentHorizontalSpeed, _currentVerticalSpeed); // Used externally
            var move = _rawMovement * Time.deltaTime;
            var furthestPoint = pos + move;

            // check furthest movement. If nothing hit, move and don't do extra checks
            var hit = Physics2D.OverlapBox(furthestPoint, _characterBounds.size, 0, _groundLayer);
            if (!hit)
            {
                transform.position += move;
                return;
            }

            // otherwise increment away from current pos; see what closest position we can move to
            var positionToMoveTo = transform.position;
            for (int i = 1; i < _freeColliderIterations; i++)
            {
                // increment to check all but furthestPoint - we did that already
                var t = (float)i / _freeColliderIterations;
                var posToTry = Vector2.Lerp(pos, furthestPoint, t);

                if (Physics2D.OverlapBox(posToTry, _characterBounds.size, 0, _groundLayer))
                {
                    transform.position = positionToMoveTo;

                    // We've landed on a corner or hit our head on a ledge. Nudge the player gently
                    if (i == 1)
                    {
                        if (_currentVerticalSpeed < 0) _currentVerticalSpeed = 0;
                        var dir = transform.position - hit.transform.position;
                        transform.position += dir.normalized * move.magnitude;
                    }

                    return;
                }

                positionToMoveTo = posToTry;
            }
        }

        private void HandleSpriteDirection()
        {
            if (_movementValue != 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(_movementValue), 1f);
            }
        }

        #endregion

        private void DeathInputLock(object sender, EventArgs e)
        {
            LockInput(true);
        }

        private void OnEnable()
        {
            HealthSystem.OnDeath += DeathInputLock;
            HealthSystem.OnHitPointsChanged += TakeDamage;
        }

        private void OnDisable()
        {
            HealthSystem.OnDeath -= DeathInputLock;
            HealthSystem.OnHitPointsChanged -= TakeDamage;
        }
    }
}
