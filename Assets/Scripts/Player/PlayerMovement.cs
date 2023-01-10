using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //stats
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpHeight;
    [SerializeField] private float _jumpCooldown;
    private float _canJump;
    
    //sprawdzanie podloza
    public float groundDrag;
    public LayerMask groundMask;
    private bool _isOnGround = true;
    
    //wymagane wartosci
    [SerializeField] private Transform _orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _direction;

    private Rigidbody _rb;
    
    [SerializeField] private float GroundcheckRayHeight=0.2f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Rzucamy promien prosto w dol aby sprawdzic czy mamy cos pod nogami
        _isOnGround = Physics.Raycast(transform.position, Vector3.down, GroundcheckRayHeight,groundMask);
        
        //branie inputu, raw bo wektor musi byc znormalizowany w funkcji move, wiec musi byc wartosc bzwzg = 1 lub 0 wiecej o tym tam
        _horizontalInput = Input.GetAxisRaw(Controls.HORIZONTAL);
        _verticalInput = Input.GetAxisRaw(Controls.VERTICAL);
        
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

    private void Move()
    {
        _direction = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        //musimy znormalizowac wektor, bo inaczej poruszanie sie po skosie jest szybsze od poruszania sie prosto
        //force mode force bo chcemy ciagle dodawac sile ruchu
        _rb.AddForce(_direction.normalized * _movementSpeed, ForceMode.Force);
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

    
}
