using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //LightManager.Instance.TurnOffLightsGlobal();
        //LightManager.Instance.TurnOffAlarm();
        SoundManager.Instance.PlaySound(Sound.Footsteps, this.transform, false);
        //SoundManager.Instance.PlaySound(Sound.BGMThing);
    }
}
