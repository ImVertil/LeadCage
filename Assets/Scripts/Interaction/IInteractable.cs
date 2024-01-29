using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    //float MaxRange { get; }

    void OnStartLook();
    void OnEndLook();
    void OnInteract(InputAction.CallbackContext ctx);
}