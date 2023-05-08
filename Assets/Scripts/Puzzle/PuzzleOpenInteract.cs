using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleOpenInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private List<StoryValue> _requiredValues = new();

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
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
            InteractionManager.Instance.InfoText.SetText("Missing an item: Screwdriver");
            StartCoroutine(TextManager.WaitAndClearInfoText());
        }
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText("Press [E] to open the electrical box");
    }
}
