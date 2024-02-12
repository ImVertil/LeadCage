using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractShip : MonoBehaviour, IInteractable
{
    private Outline _outline;

    void Start() // start because awake doesn't let the outline compute properly
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.SetInteractionText($"Press [E] to escape.");
        _outline.enabled = true;
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.SetInteractionText("");
        _outline.enabled = false;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        InteractionManager.Instance.SetInfoText("Well... There was supposed to be an ending scene but... it is what it is. Anyways, you have completed the game! Now ALT + F4 :)");
        SceneLoader.Instance.LoadEndScene();
    }

}
