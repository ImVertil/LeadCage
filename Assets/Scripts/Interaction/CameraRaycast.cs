using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private float range;

    private IInteractable currentTarget;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastForInteractable();

        if(Input.GetButtonDown(Controls.INTERACT))
        {
            if(currentTarget != null)
            {
                currentTarget.OnInteract();
            }
        }
    }

    private void RaycastForInteractable()
    {
        RaycastHit whatIHit;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out whatIHit, range))
        {
            IInteractable interactable = whatIHit.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                if(whatIHit.distance <= interactable.MaxRange)
                {
                    if(interactable == currentTarget)
                    {
                        return;
                    }
                    else if(currentTarget != null)
                    {
                        currentTarget.OnEndLook();
                        currentTarget = interactable;
                        currentTarget.OnStartLook();
                        return;
                    }
                    else
                    {
                        currentTarget = interactable;
                        currentTarget.OnStartLook();
                    }
                }
                else
                {
                    if(currentTarget != null)
                    {
                        currentTarget.OnEndLook();
                        currentTarget = null;
                        return;
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.OnEndLook();
                    currentTarget = null;
                    return;
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.OnEndLook();
                currentTarget = null;
                return;
            }
        }
    }
}
