using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractVent : MonoBehaviour, IInteractable
{
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    public List<StoryValue> requiredTags;

    private void Awake()
    {
        interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText("Press [E] to remove the vent");
    }

    public void OnEndLook()
    {
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
