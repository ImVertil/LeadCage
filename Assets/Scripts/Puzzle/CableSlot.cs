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
    public GameObject connectedCable;

    void Start()
    {
        connectedCable = null;
        isCableConnected = false;
    }
}
