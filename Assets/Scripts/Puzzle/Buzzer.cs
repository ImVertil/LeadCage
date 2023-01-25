using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour
{
    [SerializeField] private Material _materialOff;
    [SerializeField] private Material _materialOn;
    [SerializeField] private Cable _doorCable;
    [SerializeField] private Cable _doorLockCable;
    [SerializeField] private Cable _terminalCable;
    [SerializeField] private Cable _lightCable;

    private Dictionary<Cable, float> _correctVoltages;
    private Dictionary<Cable, bool> _buzzers;

    void Start()
    {
        _correctVoltages = new Dictionary<Cable, float>()
        {
            { _doorCable, Puzzle.DOOR_VOLTAGE },
            { _doorLockCable, Puzzle.DOOR_LOCK_VOLTAGE },
            { _terminalCable, Puzzle.TERMINAL_VOLTAGE },
            { _lightCable, Puzzle.LIGHT_VOLTAGE }
        };

        _buzzers = new Dictionary<Cable, bool>()
        {
            { _doorCable, false },
            { _doorLockCable, false },
            { _terminalCable, false },
            { _lightCable, false }
        };

        Cable.OnCableConnect += CheckForBuzzerActivation;
        Cable.OnCableDisconnect += DeactivateBuzzer;
    }

    private void CheckForBuzzerActivation(Cable cable)
    {
        float correctVoltage = 0;
        if (_correctVoltages.ContainsKey(cable))
            correctVoltage = _correctVoltages[cable];

        if(cable.GetSlotVoltage() > correctVoltage)
        {
            ActivateBuzzer(cable);
        }
    }

    private void ActivateBuzzer(Cable cable)
    {
        if (_buzzers.ContainsKey(cable))
        {
            _buzzers[cable] = true;
            gameObject.GetComponent<MeshRenderer>().material = _materialOn;
            // Add sound
        }
    }

    private void DeactivateBuzzer(Cable cable)
    {
        if (_buzzers.ContainsKey(cable))
        {
            _buzzers[cable] = false;
            if(!_buzzers.ContainsValue(true))
            {
                gameObject.GetComponent<MeshRenderer>().material = _materialOff;
                // Add sound
            }
        }
    }
}
