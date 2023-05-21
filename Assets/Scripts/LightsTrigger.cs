using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LightManager.Instance.TurnOffLightsGlobal();
        LightManager.Instance.TurnOffAlarm();
    }
}
