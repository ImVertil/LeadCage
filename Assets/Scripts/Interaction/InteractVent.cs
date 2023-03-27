using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        interactionUIText.SetText("Remove vent");
    }

    public void OnEndLook()
    {
        interactionUIText.SetText("");
    }

    public void OnInteract()
    {
        if (IsConditionSatisfied())
            Destroy(this.gameObject); // to be changed :)
        else
            Debug.Log("[VENT] Missing item: Wrench");
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
