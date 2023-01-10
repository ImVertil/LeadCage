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

    void Start()
    {
        isCableConnected = false;
    }
}
