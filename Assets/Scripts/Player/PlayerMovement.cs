using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //predkosci
    
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jogSpeed;
    
    //fizyka
    public float groundDrag;
    //public LayerMask groundMask; jezeli jest potrzebowane w przyszloci - wszystkie raycasty z tego korzystaly
    
    private bool _isOnGround;
    
    //wymagane wartosci
    [SerializeField] private Transform _orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Rigidbody _rb;
    
    private const float GroundcheckRayHeight=0.2f;

    

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _canJump = true;

        _startingYScale = transform.localScale.y;
    }

    void Update()
    {
        //Rzucamy promien prosto w dol aby sprawdzic czy mamy cos pod nogami
        _isOnGround = Physics.Raycast(transform.position, Vector3.down, GroundcheckRayHeight);
        
        HandleInput();
        
        StateMachine();
        
        LimitSpeed();

        //dodawanie tarcia aby postac sie nie slizgala
        if (_isOnGround)
        {
            _rb.drag = groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        Move();
        climbSteps();
    }
    
    //-----------------------------------input------------------------------------------

    private void HandleInput()
    {
        //wasd, raw bo wektor musi byc znormalizowany w funkcji move, wiec musi byc wartosc bzwzg = 1 lub 0 wiecej o tym tam
        _horizontalInput = Input.GetAxisRaw(Controls.HORIZONTAL);
        _verticalInput = Input.GetAxisRaw(Controls.VERTICAL);

        //skakanie
        if (Input.GetButton(Controls.JUMP) && _canJump && _isOnGround)
        {
            _canJump = false;
            Jump();
            StartCoroutine(DelayJump());
        }

        //kucanie
        if (Input.GetButtonDown(Controls.CROUCH))
        {
            transform.localScale = new Vector3(transform.localScale.x, _crouchYScale, transform.localScale.z);
        }

        if ((_state == PlayerMovementState.crouching) && !Input.GetButton(Controls.CROUCH))
        {
            crouchDetection();
        }
    }
    
    //-----------------------------------stany-------------------------------------------
    private PlayerMovementState _state;
    public enum PlayerMovementState
    {
        jog,
        sprint,
        crouching,
        airborne
    }

    private void StateMachine()
    {
        //sprint
        if (_isOnGround && Input.GetButton(Controls.SPRINT))
        {
            _state = PlayerMovementState.sprint;
            _movementSpeed = _sprintSpeed;
        }
        else if (_isOnGround)
        {
            _state = PlayerMovementState.jog;
            _movementSpeed = _jogSpeed;
        }
        if (_isOnGround && (Input.GetButton(Controls.CROUCH) || _forceCrouch))
        {
            _state = PlayerMovementState.crouching;
            _movementSpeed = _crouchingSpeed;
        }
    }

    //---------------------------Poruszanie-------------------------------------------------
    private Vector3 _direction;
    private float _movementSpeed;
    private void Move()
    {
        bool _isOnSlope = IsOnSlope();
        //Aplikujemy input wzgledem kierunku wzroku
        _direction = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        
        //Gdy jestesmy na powierzchni pochylej to dodajemy sile w kierunku prostopadlym do niej
        if (_isOnSlope && !_jumpingFromSlope)
        {
            _rb.AddForce(_movementSpeed * 10f * GetSlopedDirection());

            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down*70f);
            }
        }
        //musimy znormalizowac wektor, bo inaczej poruszanie sie po skosie jest szybsze od poruszania sie prosto
        //force mode force bo chcemy ciagle dodawac sile ruchu
        if(_isOnGround)
            _rb.AddForce(10f * _movementSpeed * _direction.normalized, ForceMode.Force);
        else
            _rb.AddForce(_airMoveSpeedMult * _movementSpeed * _direction.normalized, ForceMode.Force); // w powietrzu zmieniamy predkosc poruszania

        _rb.useGravity = !_isOnSlope;
    }

    private void LimitSpeed()
    {
        
        if (IsOnSlope() && !_jumpingFromSlope)
        {
            if (_rb.velocity.magnitude > _movementSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * _movementSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            
            if (flatVelocity.magnitude > _movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }
        
    }
    
    //----------------------------------skakanie----------------------------------------------
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMoveSpeedMult;
    private bool _canJump;
    private bool _jumpingFromSlope;
    private void Jump()
    {
        _jumpingFromSlope = true;
        
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpHeight, ForceMode.Impulse);
    }

    private IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
        
        if (Input.GetButtonUp(Controls.JUMP))
        {
            _jumpingFromSlope = false;
            StopCoroutine(DelayJump());
        }
    }
    
    //--------------------------------------kucanie----------------------------------------
    [SerializeField] private float _crouchingSpeed;
    [SerializeField] private float _crouchYScale;
    private float _startingYScale;
    private bool _forceCrouch = false;

    private void crouchDetection()
    {
        if (!Physics.Raycast(transform.position, Vector3.up, _startingYScale*2 + 0.2f))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startingYScale, transform.localScale.z);
            _forceCrouch = false;
        }
        else
        {
            _forceCrouch = true;
        }
    }
    
    
    //---------------------------------------rownia pochyla--------------------------------
    [SerializeField] private float _maxSlopeAngle;
    private RaycastHit _slopeRay;

    //Na pochylych powierzchniach trzeba kalkulowac kat pochylenia i stworzyc nowy wektor rownolegly do powierzchni aby ruch dzialal prawidlowo
    private bool IsOnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out _slopeRay, GroundcheckRayHeight + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeRay.normal);
            return angle<_maxSlopeAngle&&(angle != 0);
        }
        return false;
    }

    private Vector3 GetSlopedDirection()
    {
        return Vector3.ProjectOnPlane(_direction, _slopeRay.normal).normalized;
    }
    
    //-----------------------------------schody------------------------------------------
    [SerializeField] private float _maxStepHeight = 0.3f;
    [SerializeField] private float _stepJump = 0.1f;
    private float _playerCapsuleRadius = 0.5f;
    private void climbSteps()
    {
        RaycastHit stepBottomHit;
        if (_direction.x == 0 && _direction.z == 0)
        {
            return;
        }
        //Debug.DrawRay(transform.position, _direction.normalized * 0.6f);
        if (!Physics.Raycast(transform.position, _direction.normalized, out stepBottomHit, _playerCapsuleRadius + 0.1f) || IsOnSlope())
        {
            return;
        }
        
        //Wykrywanie pochylej powierzchni aby nie wykonywac skakania (Jest od tego inny system)
        if (Vector3.Angle(Vector3.forward, stepBottomHit.normal) < 90)
        {
            return;
        }
        
        RaycastHit stepTopHit;
        Vector3 topRaycastOrigin = new Vector3(transform.position.x, transform.position.y +_maxStepHeight, transform.position.z);
        //Debug.DrawRay(topRaycastOrigin, _direction.normalized * 0.7f);
        if (!Physics.Raycast(topRaycastOrigin, _direction.normalized, out stepTopHit, _playerCapsuleRadius + 0.25f))
        {
            _rb.position -= new Vector3(0f, -_stepJump, 0f);
        }
    }
    

}
