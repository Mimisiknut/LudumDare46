﻿using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float _speed = 200f;

    [SerializeField]
    private float _jumpForce = 300;

    private Rigidbody2D _rb;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _velocity = Vector3.zero;
    private PlayerControls _controls;
    private Vector2 _moveAxis;
    private bool _isJumping = false;

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Move.performed += HandleMove;
        _controls.Gameplay.Move.canceled += context => _moveAxis = Vector2.zero;

        _controls.Gameplay.Jump.performed += HandleJump;
        _controls.Gameplay.Jump.canceled += context => _isJumping = false;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        Jump();
        AnimationsMovements(_rb.velocity.x);
    }

    private void HandleMove(InputAction.CallbackContext context)
    {
        _moveAxis = context.ReadValue<Vector2>();
    }

    private void HandleJump(InputAction.CallbackContext context)
    {
        _isJumping = true;
    }

    private void MoveCharacter()
    {
        Vector3 targetVelocity = new Vector2(_moveAxis.x * Time.fixedDeltaTime * _speed, _rb.velocity.y);
        _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, .05f);             
    }


    private void Jump()
    {
        if (!_isJumping || _rb.velocity.y != 0) return;

        _rb.AddForce(new Vector2(0f, _jumpForce));
        _isJumping = false;
    }

    private void AnimationsMovements(float speedX)
    {
        if (speedX < 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }

        _animator.SetFloat("Speed", Mathf.Abs(speedX));
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        _controls.Gameplay.Disable();
    }
}