using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove = true; 
    public bool CanMoveCamera = true;
    
    private Rigidbody _rb;
    private Animator _animator;
    private bool _hasAnimator;
    private bool _grounded;

    private int _xVelHash;
    private int _yVelHash;
    private int _fallVelHash;
    private int _jumpHash;
    private int _groundedHash;
    private int _fallingHash;
    
    private Vector2 _inputVector;
    private float _xRotation;

    private Vector2 _currentVelocity;

    [SerializeField] private float WalkSpeed = 2f;
    [SerializeField] private float RunSpeed = 6f;
    [SerializeField] private float AnimBlendSpeed = 8.9f;

    [SerializeField] private float UpperCameraLimit = -40f;
    [SerializeField] private float BottomCameraLimit = 70f;

    [SerializeField] private float MouseSensitivity = 21.9f;

    [SerializeField] private float JumpStrength = 260f;
    [SerializeField] private float DistanceToGround = 0.8f;

    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _rb = GetComponent<Rigidbody>();

        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _fallVelHash = Animator.StringToHash("Fall_Velocity");
        _jumpHash = Animator.StringToHash("Jump");
        _groundedHash = Animator.StringToHash("Grounded");
        _fallingHash = Animator.StringToHash("Falling");
    }

    private void Update()
    {
        if (Input.GetButtonDown(Controls.JUMP))
        {
            JumpHandling();
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
    }

    private void LateUpdate()
    {
        CameraControl();
    }

    private void Move()
    {
        if(!_hasAnimator || !CanMove) return;

        //_horizontalInput = Input.GetAxisRaw(Controls.HORIZONTAL);
        //_verticalInput = Input.GetAxisRaw(Controls.VERTICAL);

        _inputVector = new Vector2(Input.GetAxisRaw(Controls.HORIZONTAL), Input.GetAxisRaw(Controls.VERTICAL));

        float targetSpeed = Input.GetButton(Controls.SPRINT) ? RunSpeed : WalkSpeed;

        
        if (_inputVector.x == 0 && _inputVector.y == 0) targetSpeed = 0; 
        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputVector.normalized.x, AnimBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputVector.normalized.y, AnimBlendSpeed * Time.fixedDeltaTime);

        var xVelDifference = _currentVelocity.x - _rb.velocity.x;
        var yVelDifference = _currentVelocity.y - _rb.velocity.z;
        
        _rb.AddForce(transform.TransformVector(new Vector3(xVelDifference, 0, yVelDifference)), ForceMode.VelocityChange);
        
        _animator.SetFloat(_xVelHash, _currentVelocity.x);
        _animator.SetFloat(_yVelHash, _currentVelocity.y);


    }

    private void CameraControl()
    {
        if (!_hasAnimator || !CanMoveCamera) return;

        var mouseX = Input.GetAxisRaw(Controls.MOUSEX);
        var mouseY = Input.GetAxisRaw(Controls.MOUSEY);
        Camera.position = CameraRoot.position;

        _xRotation -= mouseY * MouseSensitivity * Time.smoothDeltaTime;
        _xRotation = Mathf.Clamp(_xRotation, UpperCameraLimit, BottomCameraLimit);

        Camera.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX * MouseSensitivity * Time.smoothDeltaTime);
        //_rb.MoveRotation(_rb.rotation * Quaternion.Euler(0, mouseX * MouseSensitivity * Time.smoothDeltaTime, 0));
    }

    private void JumpHandling()
    {
        if (!_hasAnimator || !_grounded) return;
        _animator.SetTrigger(_jumpHash);
        JumpForce();
    }
    
    public void JumpForce()
    {
        _rb.AddForce(-_rb.velocity.y * Vector3.up, ForceMode.VelocityChange);
        _rb.AddForce(Vector3.up *  JumpStrength, ForceMode.Impulse);
        _animator.ResetTrigger(_jumpHash);
    }

    private void CheckGround()
    {
        if (!_hasAnimator) return;

        RaycastHit hitInfo;
        if (Physics.Raycast(_rb.worldCenterOfMass, Vector3.down, out hitInfo, DistanceToGround + 0.1f))
        {
            _grounded = true;
            SendGroundInfoToAnimator();
            return;
        }

        _grounded = false;
        _animator.SetFloat(_fallVelHash, _rb.velocity.y);
        SendGroundInfoToAnimator();
        return;
    }

    private void SendGroundInfoToAnimator()
    {
        _animator.SetBool(_fallingHash, !_grounded);
        _animator.SetBool(_groundedHash, _grounded);
    }
}