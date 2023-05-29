using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    [SerializeField] private Transform _transformToPlay;
    private bool _hasEntered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_hasEntered)
        {
            StartCoroutine(WaitAndTurnOff());
            _hasEntered = true;
        }
    }

    private IEnumerator WaitAndTurnOff()
    {
        yield return new WaitForSeconds(5);
        LightManager.Instance.TurnOffLightsGlobal();
        LightManager.Instance.TurnOffAlarm();
        SoundManager.Instance.PlaySound(Sound.Footsteps, _transformToPlay, false, 0.9f);
        SoundManager.Instance.PlaySound(Sound.PowerDown, transform, false);
        SoundManager.Instance.PlayMusic(Sound.BGMThing);
    }
}
