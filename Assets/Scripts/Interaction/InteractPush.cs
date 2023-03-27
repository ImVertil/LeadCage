using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractPush : MonoBehaviour, IInteractable
{
    public GameObject interactionTextObject;
    private TMP_Text _interactionUIText;
    [SerializeField] private float _range = 100f;
    //[SerializeField] private float _impactForce = 30f;
    [SerializeField] private Rigidbody _bodyToPush;
    [SerializeField] private GameObject _player;
    private bool _isNotAChild;


    public GameObject bulletOrigin;

    private void Awake()
    {
        _interactionUIText = interactionTextObject.GetComponent<TMP_Text>();
        _isNotAChild = true;
    }

    public void OnStartLook()
    {
        _interactionUIText.SetText($"Push");
    }

    public void OnEndLook()
    {
        _interactionUIText.SetText("");
    }

    public void OnInteract()
    {
        //popychanie
/*        RaycastHit hit;
        if (Physics.Raycast(bulletOrigin.transform.position, bulletOrigin.transform.forward, out hit, _range))
        {
            if (hit.rigidbody == _bodyToPush)
            {
                hit.rigidbody.constraints = ~RigidbodyConstraints.FreezePosition;
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
                Invoke("Freeze", 0.7f);
            }
        }*/

        //³apanie
        RaycastHit hit;
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
        }
    }

    public void Freeze()
    {
        _bodyToPush.constraints = RigidbodyConstraints.FreezeAll;
    }
}
