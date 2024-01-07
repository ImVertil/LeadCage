using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class InteractPush : MonoBehaviour, IInteractable
{
    public GameObject interactionTextObject;
    private TMP_Text _interactionUIText;
    [SerializeField] private float _range = 1000f;
    [SerializeField] private float _impactForce = 100f;
    [SerializeField] private Rigidbody _bodyToPush;
    [SerializeField] private GameObject _player;
    private bool _isNotAChild;
    private Outline _outline;


    public GameObject bulletOrigin;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        //_interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
        _isNotAChild = true;
    }

    public void OnStartLook()
    {
        InteractionManager.Instance.InteractionText.SetText($"Press [E] to push object");
        _outline.enabled = true;
        //_interactionUIText.SetText($"Push");
    }

    public void OnEndLook()
    {
        InteractionManager.Instance.InteractionText.SetText("");
        _outline.enabled = false;
        //_interactionUIText.SetText("");
    }
/*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _outline.enabled = false;
            Debug.Log("interact kurwa");
            //popychanie
            RaycastHit hit;
            if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
            {
                if (hit.rigidbody == _bodyToPush)
                {
                    Debug.Log("hit kurwa");
                    //hit.rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
                    hit.rigidbody.AddForce(-hit.normal * _impactForce);
                    //Invoke("Freeze", 0.7f);
                }
            }
        }
    }
*/
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        _outline.enabled = false;
        //Debug.Log("interact kurwa");
        //popychanie
        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
        {
            if (hit.rigidbody == _bodyToPush)
            {
                //Debug.Log("hit kurwa");
                //hit.rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
                //Invoke("Freeze", 0.7f);
            }
        }

        //³apanie
        /*RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
        {
            if (hit.rigidbody == _bodyToPush)
            {
                if (_isNotAChild)
                {
                    _bodyToPush.transform.SetParent(_player.transform);
                    _bodyToPush.constraints = ~RigidbodyConstraints.FreezePosition;
                    _isNotAChild = false;
                } else if (!_isNotAChild)
                {
                    _bodyToPush.gameObject.transform.parent = null;
                    Freeze();
                    _isNotAChild = true;
                }
            }
        }*/
    }

    /*public void Freeze()
    {
        _bodyToPush.constraints = RigidbodyConstraints.FreezeAll;
    }*/
}