using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //predkosci
    
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _jogSpeed;
    
    //fizyka
    public float groundDrag;
    public LayerMask groundMask;
    
    private bool _isOnGround;
    
    //wymagane wartosci
    [SerializeField] private Transform _orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Rigidbody _rb;
    
    private const float GroundcheckRayHeight=0.1f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _canJump = true;

        _startingYScale = transform.localScale.y;
    }

    void Update()
    {
        //Rzucamy promien prosto w dol aby sprawdzic czy mamy cos pod nogami
        _isOnGround = Physics.Raycast(transform.position, Vector3.down, GroundcheckRayHeight,groundMask);
        
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

        if (Input.GetButtonUp(Controls.CROUCH))
        {
            transform.localScale = new Vector3(transform.localScale.x, _startingYScale, transform.localScale.z);
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
        if (_isOnGround && Input.GetButton(Controls.CROUCH))
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
        _direction = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        //musimy znormalizowac wektor, bo inaczej poruszanie sie po skosie jest szybsze od poruszania sie prosto
        //force mode force bo chcemy ciagle dodawac sile ruchu
        if(_isOnGround)
            _rb.AddForce(10f * _movementSpeed * _direction.normalized, ForceMode.Force);
        else
            _rb.AddForce(_airMoveSpeedMult * _movementSpeed * _direction.normalized, ForceMode.Force);

    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
        }
    }
    
    //----------------------------------skakanie----------------------------------------------
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpCooldown;
    [SerializeField] private float _airMoveSpeedMult;
    private bool _canJump;
    private void Jump()
    {
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        _rb.AddForce(transform.up * _jumpHeight, ForceMode.Impulse);
    }

    private IEnumerator DelayJump()
    {
        yield return new WaitForSeconds(_jumpCooldown);
        _canJump = true;
        if (Input.GetButtonUp(Controls.JUMP))
        {
            StopCoroutine(DelayJump());
        }
    }
    
    //--------------------------------------kucanie----------------------------------------
    [SerializeField] private float _crouchingSpeed;
    [SerializeField] private float _crouchYScale;
    private float _startingYScale;
    
}
