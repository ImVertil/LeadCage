using System;
using UnityEngine;

public class Cable : MonoBehaviour
{
    private GameObject _currentSlot;
    public static event Action<Cable> OnCableConnect;
    public static event Action<Cable> OnCableDisconnect;

    void Start()
    {
        _currentSlot = null;
    }

    public void ConnectToSlot(GameObject slot)
    {
        _currentSlot = slot;
        OnCableConnect?.Invoke(this);
    }

    public void DisconnectFromSlot()
    {
        _currentSlot = null;
        OnCableDisconnect?.Invoke(this);
    }

    public float GetSlotVoltage()
    {
        // If it's not connected, voltage is 0
        if (_currentSlot is null)
            return 0;
        else
            return _currentSlot.GetComponent<CableSlot>().Voltage;
    }
}
