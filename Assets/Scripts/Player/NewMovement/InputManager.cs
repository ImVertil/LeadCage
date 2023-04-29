using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputManager : MonoBehaviour
{

    public static InputManager current;
    
    private PlayerInput _playerInput;
    private InputActionMap _currentMap;

    //Movement Actions
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Run { get; private set; }
    public bool Jump { get; private set; }
    public bool Crouch { get; private set; }
    
    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction RunAction;
    public InputAction JumpAction;
    public InputAction CrouchAction;
    //Combat Actions
    public bool Fire { get; private set; }
    public bool Aim { get; private set; }
    public bool Unsheathe { get; private set; }
    
    public InputAction UnsheatheAction;
    public InputAction AimAction;
    public InputAction FireAction;
    public InputAction PrimaryWeaponAction;
    public InputAction SecondaryWeaponAction;
    //Interaction
    public bool Interact { get; private set; }
    
    public InputAction InteractAction;

    // Inventory
    public InputAction InventoryToggleAction;


    private void Awake()
    {
        current = this;
        
        _playerInput = GetComponent<PlayerInput>();
        _currentMap = _playerInput.currentActionMap;
        
        InitialiseActions();
    }

        
//
    private void InitialiseActions()
    {
        MoveAction = _currentMap.FindAction("Move");
        LookAction = _currentMap.FindAction("Look"); 
        RunAction = _currentMap.FindAction("Run");
        JumpAction = _currentMap.FindAction("Jump");
        CrouchAction = _currentMap.FindAction("Crouch");
        
        FireAction = _currentMap.FindAction("Fire");
        AimAction = _currentMap.FindAction("Aim");
        UnsheatheAction = _currentMap.FindAction("Unsheathe");
        PrimaryWeaponAction = _currentMap.FindAction("Primary Weapon");
        SecondaryWeaponAction = _currentMap.FindAction("Secondary Weapon");
        
        InteractAction = _currentMap.FindAction("Interact");

        InventoryToggleAction = _currentMap.FindAction("Inventory Toggle");
        
        MoveAction.performed += OnMove;
        LookAction.performed += OnLook;
        RunAction.performed += OnRun;
        CrouchAction.performed += OnCrouch;

        FireAction.performed += OnFire;
        AimAction.performed += OnAim;

        InteractAction.performed += OnInteract;
        
        MoveAction.canceled += OnMove;
        LookAction.canceled += OnLook;
        RunAction.canceled += OnRun;
        CrouchAction.canceled += OnCrouch;
        
        FireAction.canceled += OnFire;
        AimAction.canceled += OnAim;
        
        InteractAction.canceled += OnInteract;
    
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Move = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        Run = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        Crouch = context.ReadValueAsButton();
    }
    
    private void OnFire(InputAction.CallbackContext context)
    {
        Fire = context.ReadValueAsButton();
    }

    private void OnUnsheathe(InputAction.CallbackContext context)
    {
    }
    
    private void OnAim(InputAction.CallbackContext context)
    {
        Aim = context.ReadValueAsButton();
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        Interact = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
       _currentMap.Enable(); 
    }

    private void OnDisable()
    {
       _currentMap.Disable(); 
    }
}
