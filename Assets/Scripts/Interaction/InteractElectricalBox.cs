using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractElectricalBox : MonoBehaviour, IInteractable
{
    private bool _isInteracting;
    [SerializeField] private Camera _electricalBoxCamera;
    private Camera _mainCamera;

    private void Awake()
    {
        _isInteracting = false;
        _mainCamera = Camera.main;
    }
    public void OnStartLook()
    {
        
    }

    public void OnEndLook()
    {
        
    }

    public void OnInteract()
    {
        if(_isInteracting)
        {
            _electricalBoxCamera.enabled = false;
            _mainCamera.enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            _isInteracting = false;
        }
        else
        {
            _electricalBoxCamera.enabled = true;
            _mainCamera.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _isInteracting = true;
        }
    }
}

