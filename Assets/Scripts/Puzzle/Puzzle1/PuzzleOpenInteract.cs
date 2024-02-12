using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class PuzzleOpenInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private List<StoryValue> _requiredValues = new();
    private Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        _outline.enabled = true;
        InteractionManager.Instance.SetInteractionText("Press [E] to open the electrical box");
    }

    public void OnEndLook()
    {
        _outline.enabled = false;
        InteractionManager.Instance.SetInteractionText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(Progression.Instance.HasAnyValues(_requiredValues))
        {
            GetComponentInParent<Animator>().Play("Puzzle1");
            GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            InteractionManager.Instance.SetInfoText("Missing an item: Screwdriver");
            
        }
    }

}
