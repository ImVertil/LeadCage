using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 _offset;
    private float _screenPointZ;
    private GameObject _snapTo;

    private void OnMouseDown()
    {
        _screenPointZ = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _offset;
    }

    private void OnMouseUp()
    {
        if(_snapTo is not null)
        {
            transform.position = new Vector3(_snapTo.transform.position.x, _snapTo.transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("enter");
        _snapTo = collider.gameObject;
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("exit");
        _snapTo = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _screenPointZ;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
