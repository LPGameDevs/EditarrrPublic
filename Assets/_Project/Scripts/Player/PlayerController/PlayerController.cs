using System;
using System.Linq;
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


        [field: SerializeField] private Rigidbody2D Rigidbody { get; set; }
        private HealthSystem Health { get; set; }


        Vector3 Velocity { get; set; }
        Vector3 LastTransformPosition { get; set; }



        float StunDuration { get; set; }
        bool IsStunned { get => this.StunDuration > 0; }

        void Awake()
        {
            this.Health = this.GetComponent<HealthSystem>();
            this.Animator = this.GetComponentInChildren<Animator>();
            this.AwakeExternalForce();
        }

        private void Start()
        {
            // Application.targetFrameRate = 20;

            this.StartDebug();
            this.StartCollision();
            this.StartMove();
        }

        internal override void OnEnable()
        {
            base.OnEnable();
            HealthSystem.OnDeath += this.HealthSystem_OnDeath;
            HealthSystem.OnHitPointsChanged += this.HealthSystem_OnHitPointsChanged;
            this.OnEnableDebug();
        }

        internal override void OnDisable()
        {
            base.OnDisable();

            HealthSystem.OnDeath -= this.HealthSystem_OnDeath;
            HealthSystem.OnHitPointsChanged -= this.HealthSystem_OnHitPointsChanged;
            this.OnDisableDebug();
        }

        private void Update()
        {
            if (this.IsDebugMode)
            {
                this.UpdateDebug();
            }
            else
            {
                // this.DeltaTime = (this.DeltaTime + Time.deltaTime) / 2f;
                // this.DeltaTime = Time.deltaTime;
                this.UpdateController();
            }
        }

        //private void UpdateController()
        //{
        //    if (!this._active)
        //        return;

        //    this.Velocity = this.transform.position - this.LastTransformPosition;
        //    this.LastTransformPosition = this.transform.position;


        //    this.UpdatePlatform();
        //    this.UpdateInput();
        //    this.UpdateStun();


        //    this.UpdateJumpApex();
        //    this.UpdateWalk();
        //    this.UpdateGravity();
        //    this.UpdateJump();

        //    this.UpdateMove();


        //    this.UpdateCollision();

        //    this.UpdateAnimator();
        //    this.UpdateSpriteDirection();
        //}

        private void FixedUpdate()
        {
            if (!this._active)
                return;

            this.Velocity = this.transform.position - this.LastTransformPosition;
            this.LastTransformPosition = this.transform.position;

            this.UpdateJumpApex();
            this.UpdateWalk();
            this.UpdateGravity();
            this.UpdateJump();
            this.UpdateExternalForce();
            this.UpdateMoveConstraints();

            this.UpdateMove();


            this.UpdateCollision();
        }

        private void UpdateController()
        {
            if (!this._active)
                return;

            this.UpdatePlatform();
            this.UpdateInput();
            this.UpdateStun();

            this.UpdateAnimator();
            this.UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            if (this.RawInputMove != 0)
            {
                this.transform.localScale = new Vector2(Mathf.Sign(this.RawInputMove), 1f);
            }
        }

        #region Damage
        void OnTakeDamage(float currentHealth, float stunDuration)
        {
            if (currentHealth <= 0)
                return;

            if (stunDuration > 0)
                this.SetStun(stunDuration);
        }

        private void SetStun(float time)
        {
            this.StunDuration = time.Max(this.StunDuration);
        }

        private void UpdateStun()
        {
            this.StunDuration = (this.StunDuration - Time.deltaTime).ClampMin(0);
        }


        private void HealthSystem_OnDeath(object sender, EventArgs e)
        {
            this.SetInputLock(true);
        }


        private void HealthSystem_OnHitPointsChanged(object sender, HealthSystem.OnHealthChangedArgs e)
        {
            this.OnTakeDamage(e.value, e.stunDuration);
        }
        #endregion

        #region Pausable Character

        internal override void Deactivate()
        {
            base.Deactivate();
            //_velocity = Vector3.zero;
            this.Movement = Vector3.zero;
            this.VerticalSpeed = 0f;
            this.HorizontalSpeed = 0f;
            this.IsMoving = false;
        }
        #endregion


        //private HealthSystem _health;
        //private PlayerForceReceiver _forceReceiver;

        //public bool IsMoving => _isMoving;

        //[SerializeField] Rigidbody2D rigidBody;
        //private float _currentHorizontalSpeed, _currentVerticalSpeed;

        //void Awake()
        //{
        //    _health = GetComponent<HealthSystem>();
        //    Animator = GetComponentInChildren<Animator>();
        //    _forceReceiver = GetComponent<PlayerForceReceiver>();
        //}


        //internal override void Deactivate()
        //{
        //    base.Deactivate();
        //    _velocity = Vector3.zero;
        //    _rawMovement = Vector3.zero;
        //    _currentHorizontalSpeed = 0f;
        //    _currentVerticalSpeed = 0f;
        //    _isMoving = false;
        //}

        //void Deactivate(object sender, System.EventArgs e) => Deactivate();

        //void LockInput(bool applyLock) => _inputLocked = applyLock;

        //void TakeDamage(object sender, HealthSystem.OnHealthChangedArgs healthArgs)
        //{
        //    if (healthArgs.value <= 0)
        //        return;

        //    if (healthArgs.stunDuration > 0)
        //        StartCoroutine(CoroutineStun(healthArgs.stunDuration));
        //}

        //IEnumerator CoroutineStun(float stunDuration)
        //{
        //    Debug.Log("Start Stun");
        //    LockInput(true);

        //    yield return new WaitForSeconds(stunDuration);

        //    Debug.Log("Stop Stun");
        //    LockInput(false);
        //}

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab))
        //        LockInput(!_inputLocked);


        //    if (!_active) return;
        //    // Calculate velocity
        //    _velocity = (transform.position - _lastPosition) / Time.deltaTime;
        //    _lastPosition = transform.position;

        //    GatherInput();
        //    RunCollisionChecks();

        //    CalculateWalk(); // Horizontal movement
        //    CalculateJumpApex(); // Affects fall speed, so calculate before gravity
        //    CalculateGravity(); // Vertical movement
        //    CalculateJump(); // Possibly overrides vertical
        //    CalculatePlatform();

        //    MoveCharacter(); // Actually perform the axis movement
        //    HandleSpriteDirection();

        //    UpdateAnimationVariables();
        //}

        //private void DeathInputLock(object sender, EventArgs e)
        //{
        //    LockInput(true);
        //}

        //internal override void OnEnable()
        //{
        //    base.OnEnable();
        //    HealthSystem.OnDeath += DeathInputLock;
        //    HealthSystem.OnHitPointsChanged += TakeDamage;
        //}

        //internal override void OnDisable()
        //{
        //    base.OnDisable();
        //    HealthSystem.OnDeath -= DeathInputLock;
        //    HealthSystem.OnHitPointsChanged -= TakeDamage;
        //}


        //private void OnDrawGizmos()
        //{
        //    this.OnDrawGizmos_Collision();
        //    this.OnDrawGizmos_Move();
        //}
    }
}
