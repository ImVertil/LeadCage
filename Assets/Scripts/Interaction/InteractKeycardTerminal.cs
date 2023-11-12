using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractKeycardTerminal : MonoBehaviour, IInteractable
{
    public List<StoryValue> requiredTags;
    [SerializeField] private GameObject _door;
    private Outline _outline;
    private Animator _animator;
    private AudioSource _audioSource;
    private int _trDoorOpen = Animator.StringToHash("DoorOpen");
    private int _trDoorClose = Animator.StringToHash("DoorClose");

    private void Start()
    {
        _animator = _door.GetComponentInChildren<Animator>();
        _audioSource = _door.GetComponentInChildren<AudioSource>();
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to open the doors");
        _outline.enabled = true;
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
        _outline.enabled = false;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(Progression.Instance.HasAllValues(requiredTags))
        {
            if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Door_01"))
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
        else
        {
            InteractionManager.Instance.InfoText.SetText("You don't have a valid keycard");
        }
    }

    public void OpenDoor()
    {
        _audioSource.Play();
        _animator.SetTrigger(_trDoorOpen);
    }

    public void CloseDoor()
    {
        _audioSource.Play();
        _animator.SetTrigger(_trDoorClose);
    }
}
