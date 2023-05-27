using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractVent : MonoBehaviour, IInteractable
{
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    public List<StoryValue> requiredTags;
    private Outline _outline;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
    }

    public void OnStartLook()
    {
        _outline.enabled = true;
        InteractionManager.Instance.InteractionText.SetText("Press [E] to remove the vent");
    }

    public void OnEndLook()
    {
        _outline.enabled = false;
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (IsConditionSatisfied())
            Destroy(this.gameObject); // to be changed :)
        else
        {
            InteractionManager.Instance.InfoText.SetText("You're missing a Screwdriver.");
            StartCoroutine(TextManager.WaitAndClearInfoText());
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
