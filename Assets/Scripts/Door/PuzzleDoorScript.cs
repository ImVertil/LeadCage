using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleDoorScript : MonoBehaviour, IInteractable
{
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private AudioSource _doorAudioSource;
    private bool _isPuzzleFinished = false;
    private bool _isOpen = false;

    void Start()
    {
        PuzzleEvents.Puzzle1Done += ChangePuzzleStatus;
    }
    public void OnStartLook()
    {
        InteractionManager.Instance.SetInteractionText("Press [E] to emergency open the doors");
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.SetInteractionText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(_isPuzzleFinished && !_isOpen)
        {
            _doorAudioSource.Play();
            _doorAnimator.SetTrigger("DoorOpen");
            _isOpen = true;
        }
        else
        {
            InteractionManager.Instance.SetInfoText("The door doesn't seem to react.");
        }
    }
    public void ChangePuzzleStatus(bool status) => _isPuzzleFinished = status;
}
