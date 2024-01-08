using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Puzzle3ValveInteract : MonoBehaviour, IInteractable
{
    private enum TurnType
    {
        LEFT, RIGHT
    }

    [SerializeField] private bool _isReverse;
    [SerializeField] private TurnType _turnType;
    [SerializeField] private Puzzle3Valve _valveScript;

    void Start()
    {
        
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText("Press [E] to turn the valve to the " + _turnType.ToString().ToLower());
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        int direction = _turnType == TurnType.LEFT ? -1 : 1;
        bool result = _valveScript.RotateValve(direction, _isReverse);
        if(!result)
        {
            InteractionManager.Instance.InfoText.SetText("It's not going any further than that...");
            StartCoroutine(TextManager.WaitAndClearInfoText());
        }
    }

}