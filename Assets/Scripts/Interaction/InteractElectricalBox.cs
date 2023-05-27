using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractElectricalBox : MonoBehaviour, IInteractable
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
        InteractionManager.Instance.InteractionText.SetText("Press [E] to interact");
    }

    public void OnEndLook()
    {
        _outline.enabled = false;
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        InteractionManager.Instance.InteractionText.SetText("");
        if (_isInteracting)
        {
            _electricalBoxCamera.enabled = false;
            _mainCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isInteracting = false;
            PlayerController.CanMove = true;
            PlayerController.CanMoveCamera = true;
            //PlayerController.EnablePlayerVisibility();
        }
        else
        {
            _electricalBoxCamera.enabled = true;
            _mainCamera.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _isInteracting = true;
            PlayerController.CanMove = false;
            PlayerController.CanMoveCamera = false;
            //PlayerController.DisablePlayerVisibility();
        }
    }
}

