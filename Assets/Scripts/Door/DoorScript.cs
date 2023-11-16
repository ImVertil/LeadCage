using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorScript : MonoBehaviour, IInteractable
{
    private int _trDoorOpen = Animator.StringToHash("DoorOpen");
    private int _trDoorClose = Animator.StringToHash("DoorClose");
    private Animator _animator;
    private AudioSource _audioSource;
    private string _interactionStatus;

    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Door_03_Close 0") && !_animator.IsInTransition(0))
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to {_interactionStatus}");
    }

    public void OnStartLook()
    {
        _interactionStatus = _animator.GetCurrentAnimatorStateInfo(0).IsName("Door_03_Close 0") ? "close" : "open";
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to {_interactionStatus}");
    }


    public void OpenDoor()
    {
        _audioSource.Play();
        _animator.SetTrigger(_trDoorOpen);
        _interactionStatus = "close";
    }

    public void CloseDoor()
    {
        _audioSource.Play();
        _animator.SetTrigger(_trDoorClose);
        _interactionStatus = "open";
    }
}