using System.Collections.Generic;
using UnityEngine;

public class Buzzer : MonoBehaviour
{
    [SerializeField] private GameObject _greenLight;
    [SerializeField] private GameObject _redLight;
    [SerializeField] private Material _greenMatOff;
    [SerializeField] private Material _greenMatOn;
    [SerializeField] private Material _redMatOff;
    [SerializeField] private Material _redMatOn;

    // names are leftovers from previous idea
    [SerializeField] private Cable _doorCable;
    [SerializeField] private Cable _doorLockCable;
    [SerializeField] private Cable _terminalCable;
    [SerializeField] private Cable _lightCable;
    [SerializeField] private Cable _redCable;

    private Dictionary<Cable, float> _correctVoltages;
    private Dictionary<Cable, bool> _buzzers;
    private Dictionary<Cable, bool> _properlyConnected;


    void Start()
    {
        _correctVoltages = new Dictionary<Cable, float>()
        {
            { _doorCable, Puzzle.ORANGE_CABLE_VOLTAGE },
            { _doorLockCable, Puzzle.GREEN_CABLE_VOLTAGE },
            { _terminalCable, Puzzle.BLUE_CABLE_VOLTAGE },
            { _lightCable, Puzzle.YELLOW_CABLE_VOLTAGE },
            { _redCable, Puzzle.RED_CABLE_VOLTAGE }
        };

        _buzzers = new Dictionary<Cable, bool>()
        {
            { _doorCable, false },
            { _doorLockCable, false },
            { _terminalCable, false },
            { _lightCable, false },
            { _redCable, false }
        };

        _properlyConnected = new Dictionary<Cable, bool>()
        {
            { _doorCable, false },
            { _doorLockCable, false },
            { _terminalCable, false },
            { _lightCable, false },
            { _redCable, false }
        };

        Cable.OnCableConnect += CheckForBuzzerActivation;
        Cable.OnCableConnect += CheckForGreenLampActivation;
        Cable.OnCableDisconnect += DeactivateBuzzer;
        Cable.OnCableDisconnect += CheckForGreenLampActivation;

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

    private void CheckForGreenLampActivation(Cable cable)
    {
        if (_correctVoltages.ContainsKey(cable))
        {
            float correctVoltage = _correctVoltages[cable];

            if (cable.GetSlotVoltage() == correctVoltage)
            {
                _properlyConnected[cable] = true;
            }
            else
            {
                _properlyConnected[cable] = false;
            }

            if (!_properlyConnected.ContainsValue(false))
            {
                ActivateGreenLamp();
            }
            else
            {
                DeactivateGreenLamp();
            }
        }
    }

    private void ActivateGreenLamp() => _greenLight.GetComponent<MeshRenderer>().material = _greenMatOn;
    private void DeactivateGreenLamp() => _greenLight.GetComponent<MeshRenderer>().material = _greenMatOff;

    private void ActivateBuzzer(Cable cable)
    {
        if (_buzzers.ContainsKey(cable))
        {
            if (!_buzzers.ContainsValue(true))
                SoundManager.Instance.PlaySound(Sound.Puzzle1_Buzzer, transform, true);
            _buzzers[cable] = true;
            _redLight.GetComponent<MeshRenderer>().material = _redMatOn;
        }
    }

    private void DeactivateBuzzer(Cable cable)
    {
        if (_buzzers.ContainsKey(cable))
        {
            _buzzers[cable] = false;
            if(!_buzzers.ContainsValue(true))
            {
                _redLight.GetComponent<MeshRenderer>().material = _redMatOff;
                SoundManager.Instance.StopSound(Sound.Puzzle1_Buzzer, transform);
            }
        }
    }
}
