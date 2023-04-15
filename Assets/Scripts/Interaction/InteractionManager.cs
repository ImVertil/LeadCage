using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float range;
    
    private IInteractable _interactionInterface = null;
    
    private Camera _mainCamera;
    
    //[SerializeField] private LayerMask interactableMask;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastForInteractable();

        if(InputManager.current.Interact)
        {
            if(_interactionInterface != null)
            {
                _interactionInterface.OnInteract();
            }
        }
    }

    private Collider _currentTarget;
    private void RaycastForInteractable()
    {
        RaycastHit whatIHit;

        Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);//ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out whatIHit, range))
        {
            setInterfaceToNull();
        }
        else if(whatIHit.transform.CompareTag(Tags.INTERACTABLE))
        {

            Collider selectedObject = whatIHit.collider;
            if (_currentTarget != selectedObject)
            {
                _currentTarget = selectedObject;
                _interactionInterface = null;
            }
            
            if (_interactionInterface == null)
            {
                _interactionInterface = selectedObject.GetComponent<IInteractable>();
                _interactionInterface.OnStartLook();
            }
        }
        else
        {
            setInterfaceToNull();
        }
        
    }

    private void setInterfaceToNull()
    {
        if (_interactionInterface != null) 
        {
            _currentTarget = null;
            _interactionInterface.OnEndLook();
            _interactionInterface = null;
        }
    }
}
