using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTestPlayerController : MonoBehaviour
{

    public float _speed = 5.0f;
    public float _jumpSpeed = 5.0f;


    private Rigidbody2D _playerRigidBody;

    private float _horizontalMovement = 0.0f;
    private float _verticalMovement = 0.0f;

    private bool _isMoving = false;
    private bool _isJumping = false;

    // Start is called before the first frame update
    private void Start()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJumping();
    }


    private void HandleInput()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");

        if (!Mathf.Approximately(_verticalMovement, 0.0f))
        {
            _isJumping = true;
        }

        if (!Mathf.Approximately(_horizontalMovement, 0.0f))
        {
            _isMoving = true;
        }
        else 
        {
            _isMoving = false;
        }
    }

    private void HandleMovement()
    {
        if(_isMoving)
        {
            Debug.Log("Moving!");

            Vector2 newPosition = _playerRigidBody.position;

            newPosition.x += (_horizontalMovement * _speed * Time.deltaTime);

            _playerRigidBody.MovePosition(newPosition);
        }
    }

    private void HandleJumping()
    {
        if (_isJumping)
        {
            Debug.Log("I jumped!");
            _isJumping = false;
        }
    }
}
