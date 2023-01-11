using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private Cable _buttonCable;
    private float _currentVoltage;

    void Start()
    {
        _currentVoltage = 0;
    }

    void Update()
    {
        _currentVoltage = _buttonCable.GetSlotVoltage();
    }

    public float GetVoltage()
    {
        return _currentVoltage;
    }
}
