using System.Collections;
using System.Collections.Generic;
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
            InteractionManager.Instance.InfoText.SetText("You don't have a valid keycard");
            StartCoroutine(TextManager.WaitAndClearInfoText());
        }
    }

    public IEnumerator Test()
    {
        OnEndLook();
        gameObject.tag = "Untagged";

        _keycardTerminalAnimator.Play(Animator.StringToHash("KeycardAnim"));
        yield return new WaitForSeconds(_keycardTerminalAnimator.GetCurrentAnimatorStateInfo(0).length);

        SoundManager.Instance.PlaySound(Sound.KeypadPress, transform, false);
        yield return new WaitForSeconds(SoundManager.Instance.GetAudioClip(Sound.KeypadPress).length);

        _keycardTerminalAnimator.Play(Animator.StringToHash("KeycardAnimR"));
        _doorScript.OpenDoor();
        gameObject.tag = "Interactable";

        yield return null;
    }
}
