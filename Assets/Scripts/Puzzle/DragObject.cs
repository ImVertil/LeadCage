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
    }

    private void OnMouseDown()
    {
        _screenPointZ = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - GetMouseWorldPosition();
        if(_snapTo is not null)
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
        if (_snapTo is not null && !_snapTo.GetComponent<CableSlot>().isCableConnected)
        {
            transform.position = new Vector3(_snapTo.transform.position.x, _snapTo.transform.position.y, transform.position.z);
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
        _snapTo = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider)
    {
        _snapTo = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _screenPointZ;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
