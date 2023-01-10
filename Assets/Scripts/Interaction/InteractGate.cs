using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGate : MonoBehaviour, IInteractable
{
    public float MaxRange { get { return _maxRange; } }

    private const float _maxRange = 10f;
    public GameObject interactionText;
    [SerializeField] private Animator anim;

    public void OnStartLook()
    {
        Debug.Log($"Looking at {gameObject.name}");
        interactionText.SetActive(true);
    }

    public void OnEndLook()
    {
        Debug.Log($"No longer looking at that object");
        interactionText.SetActive(false);
    }

    public void OnInteract()
    {
       anim.SetTrigger("OpenClose");
    }
}
