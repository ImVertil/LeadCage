using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableSlot : MonoBehaviour
{
    private float _voltage;
    public float Voltage 
    {
        get { return _voltage; }
        set { _voltage = value; }
    }
    public bool isCableConnected;

    // Start is called before the first frame update
    void Start()
    {
        isCableConnected = false;
    }
}
