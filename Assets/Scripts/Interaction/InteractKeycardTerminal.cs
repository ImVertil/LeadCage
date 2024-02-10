using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractKeycardTerminal : MonoBehaviour, IInteractable
{
    public List<StoryValue> requiredTags;
    [SerializeField] private GameObject _door;
    private Outline _outline;
    private Animator _animator;
    private Animator _keycardTerminalAnimator;
    private Door _doorScript;

    private void Start()
    {
        _animator = _door.GetComponentInChildren<Animator>();
        _keycardTerminalAnimator = GetComponent<Animator>();
        _doorScript = _door.GetComponent<Door>();
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.SetInteractionText($"Press [E] to open the doors");
        _outline.enabled = true;
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.SetInteractionText("");
        _outline.enabled = false;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if(Progression.Instance.HasAllValues(requiredTags))
        {
            var currentStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if(currentStateInfo.normalizedTime >= 1f)
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Door_01"))
                {
                    _doorScript.CloseDoor();
                }
                else
                {
                    StartCoroutine(Test());
                    //_doorScript.OpenDoor();
                }
            }
            
        }
        else
        {
            InteractionManager.Instance.SetInfoText("You don't have a valid keycard");
            
        }
    }

    public IEnumerator Test()
    {
        OnEndLook();
        gameObject.tag = Tags.UNTAGGED;

        _keycardTerminalAnimator.Play(Animator.StringToHash("KeycardAnim"));

        yield return null; // this is so the animator state info actually updates properly next frame
        yield return new WaitForSeconds(_keycardTerminalAnimator.GetCurrentAnimatorStateInfo(0).length);

        SoundManager.Instance.PlaySound(Sound.Keycard, transform, false);
        yield return new WaitForSeconds(SoundManager.Instance.GetAudioClip(Sound.Keycard).length);

        _keycardTerminalAnimator.Play(Animator.StringToHash("KeycardAnimR"));
        _doorScript.OpenDoor();
        gameObject.tag = "Interactable";

        yield return null;
    }
}
