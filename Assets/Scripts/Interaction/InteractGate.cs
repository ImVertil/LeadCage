using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class InteractGate : MonoBehaviour, IInteractable
{
    public float MaxRange { get { return _maxRange; } }

    private const float _maxRange = 5f;
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    [SerializeField] private DoorController _doorController;

    private void Awake()
    {
        interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
    }

    public void OnStartLook()
    {
        interactionUIText.SetText("Press button");
    }

    public void OnEndLook()
    {
        interactionUIText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(GetComponent<DoorButton>().GetVoltage() == 2.5)
        {
            _doorController.ToggleState();
        }
        else
        {
            Debug.Log("[BUTTON] Too low/high voltage");
        }
    }
}
