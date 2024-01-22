using System.Collections;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _lerpValue = 0.5f;

    [Range(0.0f, 1f)]
    [SerializeField] private float _waitValue = 0.05f;

    [SerializeField] private Material _defaultOnMat;
    [SerializeField] private MeshRenderer _lightMesh;

    private Light _light;
    private float _defaultIntensity;
    private float _defaultRange;
    private Color _defaultEmission;

    void Awake()
    {
        _light = GetComponentInChildren<Light>();
        _lightMesh.material = new Material(_defaultOnMat);
        _defaultIntensity = _light.intensity;
        _defaultRange = _light.range;
        _defaultEmission = _defaultOnMat.GetColor("_EmissionColor");
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        float timeNotBlinking, randRange, randIntensity;
        int timesBlinking;

        while (true)
        {
            timeNotBlinking = Random.Range(2.0f, 6.0f);
            timesBlinking = Random.Range(4, 10);

            for (int i = 0; i < timesBlinking; i++)
            {
                randRange = Random.Range(6f, 7f);
                randIntensity = Random.Range(0.5f, 1.5f);
                _light.intensity = Mathf.Lerp(_light.intensity, randIntensity, _lerpValue);
                _light.range = Mathf.Lerp(_light.range, randRange, _lerpValue);
                //_light.intensity = randIntensity;
                //_light.range = randRange;
                _lightMesh.material.SetColor("_EmissionColor", _defaultEmission * (randIntensity / _defaultIntensity));
                yield return new WaitForSeconds(_waitValue);
            }

            _light.intensity = _defaultIntensity;
            _light.range = _defaultRange;
            _lightMesh.material.SetColor("_EmissionColor", _defaultEmission);
            yield return new WaitForSeconds(timeNotBlinking);
        }
    }
}