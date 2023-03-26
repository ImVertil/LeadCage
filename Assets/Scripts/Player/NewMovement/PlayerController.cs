using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool CanMove = true; 
    public bool CanMoveCamera = true;
    
    private Rigidbody _rb;
    private Animator _animator;
    private CapsuleCollider _capsule;
    
    private bool _hasAnimator;
    private bool _grounded;
    private bool _crouching;
    
    private float _startHeight;
    private Vector3 _startCenter;
    private float _startRadius;

    private int _xVelHash;
    private int _yVelHash;
    private int _fallVelHash;
    private int _jumpHash;
    private int _groundedHash;
    private int _fallingHash;
    private int _crouchingHash;
    
    private Vector2 _inputVector;
    private float _xRotation;

    private Vector2 _currentVelocity;
    private float _onSlopeSpeedModifier = 1f;

    //private Vector2 _blendVelocity = Vector2.zero;
    private Vector3 _uncrouchCenterVelocity = Vector3.zero;
    private float _uncrouchHeightVelocity = 0;

    [SerializeField] private float WalkSpeed = 4f;
    [SerializeField] private float RunSpeed = 6f;
    [SerializeField] private float CrouchSpeed = 2f;
    [SerializeField] private float CrouchScale = 0.7f;
    [SerializeField] private float CrouchRadius = 0.5f;
    [SerializeField] private float AnimBlendSpeed = 8.9f;

    [SerializeField] private AnimationCurve SlopeSpeedAngles;

    [SerializeField] private float UpperCameraLimit = -40f;
    [SerializeField] private float BottomCameraLimit = 70f;

    [SerializeField] private float MouseSensitivity = 21.9f;

    [SerializeField] private float JumpStrength = 260f;
    [SerializeField] private float DistanceToGround = 0.8f;
    [SerializeField] private float DistanceToGroundFallOffset = 0.2f;
    [SerializeField] private float DistanceToCrouchCeiling = 0.7f;
    [SerializeField] private float DistanceIKFoot = 0.05f;

    [SerializeField] private float StepReachForce = 25f;

    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;

    [SerializeField] private LayerMask GroundMask;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _hasAnimator = TryGetComponent<Animator>(out _animator);
        _rb = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();

        _startHeight = _capsule.height;
        _startCenter = _capsule.center;
        _startRadius = _capsule.radius;

        _xVelHash = Animator.StringToHash("X_Velocity");
        _yVelHash = Animator.StringToHash("Y_Velocity");
        _fallVelHash = Animator.StringToHash("Fall_Velocity");
        _jumpHash = Animator.StringToHash("Jump");
        _groundedHash = Animator.StringToHash("Grounded");
        _fallingHash = Animator.StringToHash("Falling");
        _crouchingHash = Animator.StringToHash("Crouching");
    }

    private void Update()
    {
        if (Input.GetButtonDown(Controls.JUMP) && !_crouching && CanMove)
        {
            JumpHandling();
        }
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
        CrouchHandling();
        FloatCollider();
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

        if (Input.GetButton(Controls.CROUCH))
        {
            _crouching = true;
            targetSpeed = CrouchSpeed;
            _capsule.radius = CrouchRadius;
            if (!_grounded) //TODO, jezeli tego nie ma to crouch jumping sprawia ze mozemy wejsc w rozne niepozadane miejsca przez floating collider
            {
                _capsule.height = CrouchScale * 1.78f; 
                _capsule.center = new Vector3(0, CrouchScale*0.89f, 0);
            }
            else
            {
                _capsule.height = CrouchScale * _startHeight;
                _capsule.center = CrouchScale * _startCenter;
            }

        }
        else if (!Physics.Raycast(_rb.worldCenterOfMass, Vector3.up, DistanceToCrouchCeiling) )
        {
            _crouching = false;
            _capsule.radius = _startRadius;
            _capsule.height = Mathf.SmoothDamp(_capsule.height, _startHeight, ref _uncrouchHeightVelocity, 0.1f);
            _capsule.center = Vector3.SmoothDamp(_capsule.center, _startCenter, ref _uncrouchCenterVelocity, 0.1f);
        }


        if (_inputVector.x == 0 && _inputVector.y == 0) targetSpeed = 0;

        targetSpeed = targetSpeed * _onSlopeSpeedModifier;

        _currentVelocity.x = Mathf.Lerp(_currentVelocity.x, targetSpeed * _inputVector.normalized.x, AnimBlendSpeed * Time.fixedDeltaTime);
        _currentVelocity.y = Mathf.Lerp(_currentVelocity.y, targetSpeed * _inputVector.normalized.y, AnimBlendSpeed * Time.fixedDeltaTime);
        //_currentVelocity = Vector2.SmoothDamp(_currentVelocity, new Vector2(targetSpeed, targetSpeed) *_inputVector.normalized, ref _blendVelocity, 1/AnimBlendSpeed);
        
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

       
        if (Physics.Raycast(_rb.worldCenterOfMass, Vector3.down, out RaycastHit hitInfo, DistanceToGround + DistanceToGroundFallOffset, GroundMask ,QueryTriggerInteraction.Ignore))
        {
            _grounded = true;
            SendGroundInfoToAnimator();
            //FloatCollider(hitInfo);
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

    private void CrouchHandling()
    {
        _animator.SetBool(_crouchingHash, _crouching);
    }

    private void FloatCollider()
    {
        Vector3 capsuleCenterInWorldSpace = _capsule.bounds.center;

        Ray downFromCapsuleCenter = new Ray(capsuleCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downFromCapsuleCenter, out RaycastHit hit, DistanceToGround, GroundMask, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downFromCapsuleCenter.direction);

            SetSlopeSpeedModifierOnSlope(groundAngle);

            if (_onSlopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = _capsule.center.y - hit.distance;
            

            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * StepReachForce - _rb.velocity.y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            _rb.AddForce(liftForce, ForceMode.VelocityChange);
        }

    }

    private void SetSlopeSpeedModifierOnSlope(float angle)
    {
        _onSlopeSpeedModifier = SlopeSpeedAngles.Evaluate(angle);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_hasAnimator) return;
        
        AdjustFoot(AvatarIKGoal.LeftFoot);
        AdjustFoot(AvatarIKGoal.RightFoot);
    }

    private void AdjustFoot(AvatarIKGoal foot)
    {
        _animator.SetIKPositionWeight(foot, 1f);
        _animator.SetIKRotationWeight(foot, 1f);

        RaycastHit hit;
     
        
        float reach = 1f;

        if (_onSlopeSpeedModifier < 1 && _onSlopeSpeedModifier > 0) reach = 1.5f;
        
        Ray ray = new Ray(_animator.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, DistanceIKFoot + reach, GroundMask, QueryTriggerInteraction.Ignore))
        {
            //if (hit.distance > 1.001f + DistanceIKFoot) return;
            
            Vector3 footPosition = hit.point;
            footPosition.y += DistanceIKFoot;
            _animator.SetIKPosition(foot, footPosition);
            Vector3 fwd = Vector3.ProjectOnPlane(transform.forward, hit.normal);
            _animator.SetIKRotation(foot, Quaternion.LookRotation(fwd, hit.normal));
        }
    }
    
}