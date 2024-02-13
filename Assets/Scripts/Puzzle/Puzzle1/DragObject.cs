using System;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 _initialPos;
    private Vector3 _offset;
    private float _screenPointZ;
    private GameObject _snapTo;

    void Start()
    {
        _initialPos = transform.position;
        _snapTo = null;
    }

    private void OnMouseDown()
    {
        // screenPointZ - the object's Z screen position. Needed when we convert mouse world pos to have sync with with the object we're dragging
        _screenPointZ = Camera.main.WorldToScreenPoint(transform.position).z;

        // offset - difference between the middle of the object that we are dragging and our cursor position
        _offset = transform.position - GetMouseWorldPosition();

        if(_snapTo != null)
        {
            _snapTo.GetComponent<CableSlot>().isCableConnected = false;
            GetComponentInParent<Cable>().DisconnectFromSlot();
        }
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _offset;
    }

    private void OnMouseUp()
    {
        // snap the cable to the slot if there's no other cable connected, otherwise snap back to initial position
        if (_snapTo != null && !_snapTo.GetComponent<CableSlot>().isCableConnected)
        {
            // i really don't know how to make it better
            switch (Math.Abs(transform.root.eulerAngles.y)) // the Y rotation of the root component
            {
                case 0:
                case 180:
                case 360:
                    transform.position = new Vector3(_snapTo.transform.position.x, _snapTo.transform.position.y, transform.position.z);
                    break;

                case 90:
                case 270:
                default:
                    transform.position = new Vector3(transform.position.x, _snapTo.transform.position.y, _snapTo.transform.position.z);
                    break;
            }
            _snapTo.GetComponent<CableSlot>().isCableConnected = true;
            GetComponentInParent<Cable>().ConnectToSlot(_snapTo);
        }
        else
        {
            transform.position = _initialPos;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //Debug.Log(collider.gameObject.name);
        _snapTo = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider)
    {
        _snapTo = null;
    }

    // Converting the mouse position on screen to the world position
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _screenPointZ;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
