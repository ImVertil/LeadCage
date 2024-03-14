using System.Collections;
using UnityEngine;

public class LightsTrigger : MonoBehaviour
{
    [SerializeField] private Transform _transformToPlay;
    [SerializeField] private GameObject[] _areasToDisable;
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
        yield return new WaitForSeconds(7);
        SoundManager.Instance.PlaySound(Sound.PowerDown, transform, false);
        LightManager.Instance.TurnOffLightsGlobal();

        LightManager.Instance.TurnOffAlarm();
        SoundManager.Instance.PlaySound(Sound.GeneratorDown, _transformToPlay, false, 0.9f);
        SoundManager.Instance.PlayMusic(Sound.BGMThing);
        foreach (var obj in _areasToDisable)
            obj.SetActive(false);
    }
}
