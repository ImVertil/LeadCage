using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject _doorCable;
    [SerializeField] private GameObject _lockCable;
    private Animator _animator;
    private float _currentDoorVoltage;
    private float _currentLockVoltage;
    private bool _isOpen;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _isOpen = false;
        _currentDoorVoltage = 0;
        _currentLockVoltage = 0;
    }

    void Update()
    {
        _currentDoorVoltage = _doorCable.GetComponent<Cable>().GetSlotVoltage();
        _currentLockVoltage = _lockCable.GetComponent<Cable>().GetSlotVoltage();
    }

    private void Open()
    {
        switch(_currentDoorVoltage)
        {
            case 0:
                Debug.Log("No power (0)");
                break;
            case 2.5f:
                Debug.Log("Not enough power. (2.5)");
                break;
            case 5:
                Debug.Log("Not enough power. (5)");
                break;
            case 10:
                Debug.Log("Not enough power. (10)");
                break;
            case 15:
                Debug.Log("Not enough power. (15)");
                break;
            case 30:
                _animator.Play("DoorOpen", 0);
                _isOpen = true;
                break;

        }
    }

    private void Close()
    {
        switch (_currentDoorVoltage)
        {
            case 0:
                Debug.Log("No power (0)");
                break;
            case 2.5f:
                Debug.Log("Not enough power. (2.5)");
                break;
            case 5:
                Debug.Log("Not enough power. (5)");
                break;
            case 10:
                Debug.Log("Not enough power. (10)");
                break;
            case 15:
                Debug.Log("Not enough power. (15)");
                break;
            case 30:
                _animator.Play("DoorClose", 0);
                _isOpen = false;
                break;
        }        
    }

    public void ToggleState()
    {
        if(_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
