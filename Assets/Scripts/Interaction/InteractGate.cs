using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractGate : MonoBehaviour, IInteractable
{
    public float MaxRange { get { return _maxRange; } }

    private const float _maxRange = 5f;
    public GameObject interactionTextObject;
    private TMP_Text interactionUIText;
    [SerializeField] private Animator anim;

    private void Awake()
    {
        interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
    }

    public void OnStartLook()
    {
        interactionUIText.SetText("Open Door");
    }

    public void OnEndLook()
    {
        interactionUIText.SetText("");
    }

    public void OnInteract()
    {
       anim.SetTrigger("OpenClose");
    }
}
