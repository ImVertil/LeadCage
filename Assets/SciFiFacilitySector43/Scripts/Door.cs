using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{
    private int _trDoorOpen = Animator.StringToHash("DoorOpen");
    private int _trDoorClose = Animator.StringToHash("DoorClose");
    private Animator _animator;
    private AudioSource _audioSource;
    private bool _isOpen;

	void Start() 
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
	}


	void OnTriggerEnter(Collider c) 
    {
        if(c.tag.Equals(Tags.PLAYER))
        {
            if (_isOpen)
            {
                StopAllCoroutines();
            }
            else
            {
                OpenDoor();
            }
        }
    }

	void OnTriggerExit(Collider c) 
    {
        if(c.tag.Equals(Tags.PLAYER))
        {
            StartCoroutine(Test());
        }
	}

    public void OpenDoor() 
    {
        _isOpen = true;
        _audioSource.Play();
        _animator.SetTrigger(_trDoorOpen);
    }

    public void CloseDoor() {
        _isOpen = false;
        _audioSource.Play();
        _animator.SetTrigger(_trDoorClose);
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(2);
        CloseDoor();
    }
}
