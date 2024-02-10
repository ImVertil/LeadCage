using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractVent : MonoBehaviour, IInteractable
{
    public List<StoryValue> requiredTags;
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        _outline.enabled = true;
        InteractionManager.Instance.SetInteractionText("Press [E] to remove the vent");
    }

    public void OnEndLook()
    {
        _outline.enabled = false;
        InteractionManager.Instance.SetInteractionText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (IsConditionSatisfied())
            gameObject.SetActive(false);
        else
        {
            //InteractionManager.Instance.SetInfoText($"You are missing a Wrench");
            //TextManager.Instance.SetInfoText("You are missing a Wrench");
            InteractionManager.Instance.SetInfoText($"You are missing a Wrench");
            //
            //TextManager.ClearInfoText();
        }
    }

    private bool IsConditionSatisfied()
    {
        if(Progression.Instance.HasAllValues(requiredTags))
        {
            return true;
        }
        return false;
    }
}
