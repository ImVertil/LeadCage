using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Cable _doorCable;
    [SerializeField] private Cable _lockCable;
    private Animator _animator;
    private float _currentDoorVoltage;
    private float _currentLockVoltage;
    private bool _isOpen;
    private bool _isLocked;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _isOpen = false;
        _currentDoorVoltage = 0;
        _currentLockVoltage = 0;
    }

    void Update()
    {
        _currentDoorVoltage = _doorCable.GetSlotVoltage();
        _currentLockVoltage = _lockCable.GetSlotVoltage();
        Open();
    }

    private void Open()
    {
        if(CheckVoltages())
        {
            //_animator.Play("DoorOpen", 0);
            _isOpen = true;
            SoundManager.Instance.PlaySound(Sound.Door_Open, transform, false);
        }
    }

    private void Close()
    {
        if(CheckVoltages())
        {
            //_animator.Play("DoorClose", 0);
            _isOpen = false;
            SoundManager.Instance.PlaySound(Sound.Door_Close, transform, false);
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

    // Handles sound cues depending on voltage (not yet implemented)
    // Returns true if both lock voltage and door voltage is correct
    private bool CheckVoltages()
    {
        switch (_currentLockVoltage)
        {
            case 5:
                _isLocked = false;
                break;
            default:
                _isLocked = true;
                break;
        }

        switch (_currentDoorVoltage)
        {
            case 0:
                Debug.Log("[DOOR] No power (0)");
                break;
            case 2.5f:
                Debug.Log("[DOOR] Not enough power. (2.5)");
                break;
            case 5:
                Debug.Log("[DOOR] Not enough power. (5)");
                break;
            case 10:
                Debug.Log("[DOOR] Not enough power. (10)");
                break;
            case 15:
                Debug.Log("[DOOR] Not enough power. (15)");
                break;
            case 30:
                if (_isLocked)
                {
                    Debug.Log("[DOOR] Enough power, but locked.");
                    break;
                }
                else
                {
                    Debug.Log("[DOOR] Enough power, opening/closing.");
                }
                return true;
        }
        return false;
    }
}
