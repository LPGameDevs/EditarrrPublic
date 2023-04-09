using System;
using System.Collections.Generic;
using Editarrr.Input;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField] private InputValue MoveInput { get; set; }
        [field: SerializeField] private InputValue JumpInput { get; set; }

        private bool _isMoving;
        private float _movementValue;
        private bool _isJumping;

        private float _verticalSpeed = 0f;
        private float _horizontalSpeed = 0f;

        #region Collision detection parameters

        [Tooltip("Layer(s) to interact with for neutral collisions")]
        [SerializeField] private LayerMask groundMask = 0;
        [Tooltip("Bounding box to use for collision detection")]
        [SerializeField] private Bounds collisionBounds;
        private Vector2 _projectedPosition;
        private Boolean _isGrounded;
        private Direction2D<bool> _collisions = new Direction2D<bool>(false);
        [SerializeField][Range(0, 0.5f)] private float _raycastInset = 0.05f;
        [SerializeField][Range(0, 10)] private int _raycastCount = 3;
        [SerializeField][Range(0, 0.5f)] private float _raycastDistance = 0.05f;
        private Direction2D<Vector2[]> _rays = new Direction2D<Vector2[]>(null);

        #endregion

        #region Player control levers

        [Tooltip("Maximum horizontal speed player can move")]
        [SerializeField] private float maxSpeed = 5f;
        [Tooltip("Time to accelerate to full speed")]
        [SerializeField][Range(0, 1f)] private float acceleration = 5f;
        [Tooltip("Time to stop after a movement input ends")]
        [SerializeField][Range(0, 1f)] private float deceleration = 5f;
        [Tooltip("Force applied while jump input active")]
        [SerializeField] private float jumpForce = 5f;
        [Tooltip("Gravity")]
        [SerializeField][Range(0, 100f)] private float gravity = 9f;
        [Tooltip("Speed to clamp player falling velocity")]
        [SerializeField] private float maxFallSpeed = 5f;

        #endregion

        private Rigidbody2D rb;

        private void Awake()
        {

        }

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();

        }

        private void Update()
        {
            // We collect input during Update as it runs every frame.
            HandleInput();
            DetectCollisions();
        }

        private void FixedUpdate()
        {

            _projectedPosition = Vector2.zero;
            CalculateHorizontalMovement();
            // CalculateGravityMovement();
            HandleSpriteDirection();
            HandleGravity();
            HandleJumping();
        }

        private void DetectCollisions()
        {
            float bottom = collisionBounds.min.y + transform.position.y;
            float top = collisionBounds.max.y + transform.position.y;
            float left = collisionBounds.min.x + transform.position.x;
            float right = collisionBounds.max.x + transform.position.x;

            Vector2[] GenerateRayOrigins(Vector2 from, Vector2 to)
            {
                Vector2[] rays = new Vector2[_raycastCount];
                for (int i = 0; i < _raycastCount; i++)
                {
                    float t = (float)i / (_raycastCount - 1);
                    rays[i] = Vector2.Lerp(from, to, t);
                }
                return rays;
            }

            bool PerformRaycasts(Vector2[] origins, Vector2 direction)
            {
                foreach (var origin in origins)
                {
                    RaycastHit2D hit = Physics2D.Raycast(origin, direction, _raycastDistance, groundMask);
                    if (hit)
                    {
                        return true;
                    }
                }
                return false;
            }

            _rays.up = GenerateRayOrigins(new Vector2(left + _raycastInset, top), new Vector2(right - _raycastInset, top));
            _collisions.up = PerformRaycasts(_rays.up, Vector2.up);

            _rays.left = GenerateRayOrigins(new Vector2(left, top - _raycastInset), new Vector2(left, bottom + _raycastInset));
            _collisions.left = PerformRaycasts(_rays.left, Vector2.left);

            _rays.down = GenerateRayOrigins(new Vector2(left + _raycastInset, bottom), new Vector2(right - _raycastInset, bottom));
            _collisions.down = PerformRaycasts(_rays.down, Vector2.down);

            _rays.right = GenerateRayOrigins(new Vector2(right, top - _raycastInset), new Vector2(right, bottom + _raycastInset));
            _collisions.right = PerformRaycasts(_rays.right, Vector2.right);
        }

        private void HandleSpriteDirection()
        {
            if (_movementValue != 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(_movementValue), 1f);
            }
        }

        private void HandleInput()
        {
            _isMoving = MoveInput.IsPressed;
            _movementValue = MoveInput.Read<Vector2>().x;
            if (JumpInput.WasPressed)
            {
                _isJumping = true;
            }
        }


        private void CalculateHorizontalMovement()
        {
            if (!_isMoving)
            {
                return;
            }

            float horizontal = _movementValue;

            float movement = horizontal * maxSpeed * Time.fixedDeltaTime;

            if ((movement > 0 && _collisions.right) || (movement < 0 && _collisions.left))
            {
                Debug.Log("colli");
                return;
            }
            Vector2 newPosition = rb.position + new Vector2(movement, 0f);
            rb.MovePosition(newPosition);
        }

        private void HandleJumping()
        {
            if (!_isJumping)
            {
                return;
            }
            Debug.Log("jump");
            _isJumping = false;
        }

        private void HandleGravity()
        {
            if (_collisions.down)
            {
                _verticalSpeed = 0;
                return;
            }

            _verticalSpeed = Mathf.Clamp(_verticalSpeed - gravity, -maxFallSpeed, maxFallSpeed);
            Vector2 newPosition = rb.position + new Vector2(0, _verticalSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
        }

        private void OnDrawGizmos()
        {
            // Collision bounding box
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + collisionBounds.center, collisionBounds.size);

            if (_rays.down != null)
            {
                Gizmos.color = _collisions.down ? Color.red : Color.green;
                foreach (Vector2 r in _rays.down)
                {
                    Gizmos.DrawLine(r, r + (Vector2.down * _raycastDistance));
                }
                Gizmos.color = _collisions.up ? Color.red : Color.green;
                foreach (Vector2 r in _rays.up)
                {
                    Gizmos.DrawLine(r, r + (Vector2.up * _raycastDistance));
                }
                Gizmos.color = _collisions.left ? Color.red : Color.green;
                foreach (Vector2 r in _rays.left)
                {
                    Gizmos.DrawLine(r, r + (Vector2.left * _raycastDistance));
                }
                Gizmos.color = _collisions.right ? Color.red : Color.green;
                foreach (Vector2 r in _rays.right)
                {
                    Gizmos.DrawLine(r, r + (Vector2.right * _raycastDistance));
                }

            }
        }
    }

}
