using System.Collections;
using System.Collections.Generic;
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
    }

    public void DisconnectFromSlot()
    {
        _currentSlot = null;
    }

    public float GetSlotVoltage()
    {
        if (_currentSlot is null)
            return 0;
        else
            return _currentSlot.GetComponent<CableSlot>().Voltage;
    }
}
