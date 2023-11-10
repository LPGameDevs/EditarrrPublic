using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Editarrr.Input;
using Editarrr.Level.Tiles;
using Gameplay.GUI;
using Systems;
using Twitch;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Adapted from Tarodev's Ultimate 2D controller, found here: https://github.com/Matthew-J-Spencer/Ultimate-2D-Controller
    /// </summary>
    [RequireComponent(typeof(HealthSystem), typeof(PlayerForceReceiver))]
    public partial class PlayerController : PausableCharacter
    {
        // events
        public static event Action OnPlayerJumped;
        public static event Action OnPlayerStartedFalling;
        public static event Action<float> OnPlayerLanded;
        public static event Action<bool> OnPlayerMoved;

        private HealthSystem _health;
        private PlayerForceReceiver _forceReceiver;

        public bool IsMoving => _isMoving;

        [SerializeField] Rigidbody2D rigidBody;
        private float _currentHorizontalSpeed, _currentVerticalSpeed;

        void Awake()
        {
            _health = GetComponent<HealthSystem>();
            Animator = GetComponentInChildren<Animator>();
            _forceReceiver = GetComponent<PlayerForceReceiver>();
        }


        internal override void Deactivate()
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
            CalculatePlatform();

            MoveCharacter(); // Actually perform the axis movement
            HandleSpriteDirection();

            UpdateAnimationVariables();
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
            else if (_twitchJumpRequested)
            {
                _jumpStartThisFrame = true;
                _lastJumpPressed = Time.time;
            }
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

        private void DeathInputLock(object sender, EventArgs e)
        {
            LockInput(true);
        }

        internal override void OnEnable()
        {
            base.OnEnable();
            JumpCommand.OnTwitchJump += TwitchJump;
            BouncyCommand.OnTwitchJumpForever += TwitchBouncy;
            HealthSystem.OnDeath += DeathInputLock;
            HealthSystem.OnHitPointsChanged += TakeDamage;
        }

        internal override void OnDisable()
        {
            base.OnDisable();
            JumpCommand.OnTwitchJump -= TwitchJump;
            BouncyCommand.OnTwitchJumpForever -= TwitchBouncy;
            HealthSystem.OnDeath -= DeathInputLock;
            HealthSystem.OnHitPointsChanged -= TakeDamage;
        }


        private void OnDrawGizmos()
        {
            this.OnDrawGizmos_Collision();
            this.OnDrawGizmos_Move();
        }
    }
}
