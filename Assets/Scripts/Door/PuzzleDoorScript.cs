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
    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText("Press [E] to emergency open the doors");
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
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
                InteractionManager.Instance.InfoText.SetText("The door doesn't seem to react.");
                StartCoroutine(TextManager.WaitAndClearInfoText());
            }
        }
    }

    public void ChangeDoorLockCableState(Cable c) => _lockCable = c;
    public void ChangeDoorCableState(Cable c) => _doorCable = c;
}
