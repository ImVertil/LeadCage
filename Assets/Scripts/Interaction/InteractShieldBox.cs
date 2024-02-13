using UnityEngine;
using UnityEngine.InputSystem;

public class InteractShieldBox : MonoBehaviour, IInteractable
{
    private bool _isInteracting;
    [SerializeField] private Camera _electricalBoxCamera;
    private Camera _mainCamera;
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _isInteracting = false;
        _mainCamera = Camera.main;
    }
    public void OnStartLook()
    {
        _outline.enabled = true;
        InteractionManager.Instance.SetInteractionText("Press [E] to interact");
    }

    public void OnEndLook()
    {
        _outline.enabled = false;
        InteractionManager.Instance.SetInteractionText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        InteractionManager.Instance.SetInteractionText("");
        if (_isInteracting)
        {
            _electricalBoxCamera.enabled = false;
            _mainCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isInteracting = false;
            InputManager.OnUnfreezeMovement();
            InputManager.OnEnableShooting();
        }
        else
        {
            _electricalBoxCamera.enabled = true;
            _mainCamera.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _isInteracting = true;
            InputManager.OnFreezeMovement();
            InputManager.OnDisableShooting();
            InputManager.OnForceGunAway();
        }
    }
}
