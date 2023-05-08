using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleDoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Cable _lockCable;
    [SerializeField] private Cable _doorCable;
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private AudioSource _doorAudioSource;

    void Start()
    {
        PuzzleEvents.DoorLockCableChanged += ChangeDoorLockCableState;
        PuzzleEvents.DoorCableChanged += ChangeDoorCableState;
    }

    public void OnEndLook()
    {
        
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(_lockCable != null || _doorCable != null)
        {
            if (_lockCable.GetSlotVoltage() == Puzzle.DOOR_LOCK_VOLTAGE && _doorCable.GetSlotVoltage() == Puzzle.DOOR_VOLTAGE)
            {
                _doorAudioSource.Play();
                _doorAnimator.SetTrigger("DoorOpen");
            }
            else
            {
                InteractionManager.Instance.InfoText.SetText("Seems that the door is locked.");
                StartCoroutine(TextManager.WaitAndClearInfoText());
            }
        }
    }

    public void OnStartLook()
    {
        
    }

    public void ChangeDoorLockCableState(Cable c)
    {
        _lockCable = c;
    }
    public void ChangeDoorCableState(Cable c)
    {
        _doorCable = c;
    }
}
