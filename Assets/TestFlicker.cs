using System.Collections;
using UnityEngine;

public class TestFlicker : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _clampValue;
    [Range(0.0f, 2f)]
    [SerializeField] private float _waitValue;
    private Light _light;

    void Awake()
    {
        _light = GetComponent<Light>();
        StartCoroutine(Test());
    }

    void Update()
    {
        
    }

    private IEnumerator Test()
    {
        float randRange;
        float randIntensity;
        for(int i=0; i<1000; i++)
        {
            randRange = Random.Range(0.1f, 4.0f);
            randIntensity = Random.Range(1f, 10.0f);
            _light.intensity = randIntensity;
            _light.range = randRange;
            //_light.intensity = Mathf.Lerp(_light.intensity, randIntensity, _clampValue);
            //_light.range = Mathf.Lerp(_light.range, randRange, _clampValue);
            yield return new WaitForSeconds(_waitValue);
        }
        yield return null;
    }
}