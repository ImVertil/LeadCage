using UnityEngine;

public class DragObject : MonoBehaviour
{
    private Vector3 _offset;
    private float _screenPointZ;

    private void OnMouseDown()
    {
        _screenPointZ = Camera.main.WorldToScreenPoint(transform.position).z;
        _offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + _offset;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = _screenPointZ;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
