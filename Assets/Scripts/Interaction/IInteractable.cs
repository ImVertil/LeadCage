using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    //float MaxRange { get; }

    void OnStartLook();
    void OnInteract(InputAction.CallbackContext ctx);
    void OnEndLook();
}