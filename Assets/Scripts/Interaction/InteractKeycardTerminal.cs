using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractKeycardTerminal : MonoBehaviour, IInteractable
{
    public List<StoryValue> requiredTags;
    [SerializeField] private GameObject _door;
    private Outline _outline;
    private Animator _animator;
    private Door _doorScript;

    private void Start()
    {
        _animator = _door.GetComponentInChildren<Animator>();
        _doorScript = _door.GetComponent<Door>();
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
                _doorScript.CloseDoor();
            }
            else
            {
                _doorScript.OpenDoor();
            }
        }
        else
        {
            InteractionManager.Instance.InfoText.SetText("You don't have a valid keycard");
            StartCoroutine(TextManager.WaitAndClearInfoText());
        }
    }
}
