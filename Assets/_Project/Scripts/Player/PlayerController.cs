using System;
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

        public float speed = 5f;
        public float jumpForce = 5f;

        private Rigidbody2D rb;


        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // We collect input during Update as it runs every frame.
            HandleInput();
        }

        private void FixedUpdate()
        {
            HandleMovement();
            HandleJumping();
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


        private void HandleMovement()
        {
            if (!_isMoving)
            {
                return;
            }

            float horizontal = _movementValue;

            float movement = horizontal * speed * Time.fixedDeltaTime;
            Vector2 newPosition = rb.position + new Vector2(movement, 0f);
            rb.MovePosition(newPosition);

            if (horizontal != 0)
            {
                transform.localScale = new Vector2(Mathf.Sign(horizontal), 1f);
            }
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
    }
}
