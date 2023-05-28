using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _lerpValue = 0.5f;

    [Range(0.0f, 1f)]
    [SerializeField] private float _waitValue = 0.05f;

    [SerializeField] private Material _lightFlickerMat;
    [SerializeField] private Material _defaultOnMat;

    private Light _light;
    private float _defaultIntensity;
    private float _defaultRange;
    private Color _defaultEmission;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        _defaultIntensity = _light.intensity;
        _defaultRange = _light.range;
        _defaultEmission = _defaultOnMat.GetColor("_EmissionColor");
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        float timeNotBlinking, randRange, randIntensity;
        int timesBlinking;

        while(true)
        {
            timeNotBlinking = Random.Range(1.0f, 5.0f);
            timesBlinking = Random.Range(4, 10);

            for(int i=0; i<timesBlinking; i++)
            {
                randRange = Random.Range(6f, 7.5f);
                randIntensity = Random.Range(1f, 4f);
                _light.intensity = Mathf.Lerp(_light.intensity, randIntensity, _lerpValue);
                _light.range = Mathf.Lerp(_light.range, randRange, _lerpValue);
                _lightFlickerMat.SetColor("_EmissionColor", _defaultEmission * (randIntensity / _defaultIntensity));
                yield return new WaitForSeconds(_waitValue);
            }

            _light.intensity = _defaultIntensity;
            _light.range = _defaultRange;
            _lightFlickerMat.SetColor("_EmissionColor", _defaultEmission);
            yield return new WaitForSeconds(timeNotBlinking);
        }
    }
}