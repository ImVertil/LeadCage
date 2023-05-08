using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorScript : MonoBehaviour, IInteractable
{
    private int _trDoorOpen = Animator.StringToHash("DoorOpen");
    private int _trDoorClose = Animator.StringToHash("DoorClose");
    private Animator _animator;
    private AudioSource _audioSource;

    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _audioSource = GetComponentInParent<AudioSource>();
    }

    public void OnEndLook()
    {

    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Door_03_Close 0"))
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OnStartLook()
    {

    }


    public void OpenDoor()
    {
        Debug.Log("Open");
        _audioSource.Play();
        _animator.SetTrigger(_trDoorOpen);
    }

    public void CloseDoor()
    {
        Debug.Log("Close");
        _audioSource.Play();
        _animator.SetTrigger(_trDoorClose);
    }
}