using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;

    [SerializeField] private Transform _playerBody;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private float _xRotation = 0f;
    private float _yRotation = 0f;
    void Update()
    {
        float mouseX = Input.GetAxis(Controls.MOUSEX) * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis(Controls.MOUSEY) * _mouseSensitivity * Time.deltaTime;

        // Troche wydaje sie na odwrot, ale mouseX/Y maja wartosc wzdluz osi a rotacja dziala "Wokol" osi
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        _yRotation += mouseX;

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        _playerBody.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
