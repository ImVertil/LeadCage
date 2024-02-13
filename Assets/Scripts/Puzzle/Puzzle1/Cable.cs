using System;
using UnityEngine;

public class Cable : MonoBehaviour
{
    private GameObject _currentSlot;

    void Start()
    {
        _currentSlot = null;
    }

    public void ConnectToSlot(GameObject slot)
    {
        _currentSlot = slot;
        PuzzleEvents.OnCableConnected(this);
    }

    public void DisconnectFromSlot()
    {
        _currentSlot = null;
        PuzzleEvents.OnCableDisconnected(this);
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
