using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;
    private Material _lightsMat => _lightsMatOn;
    [SerializeField] private GameObject _alarms;
    [SerializeField] private Material _lightsMatOn;
    [SerializeField] private Material _lightsMatOff;
    [SerializeField] private GameObject[] _lights;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void TurnOnLights(GameObject[] pointLights, GameObject[] prefabLights)
    {
        foreach (var lightsObj in pointLights)
        {
            lightsObj.SetActive(true);
        }

        foreach(var prefabLightsObj in prefabLights)
        {
            foreach(var mr in prefabLightsObj.GetComponentsInChildren<MeshRenderer>())
            {
                if(mr.materials.Length == 1)
                {
                    mr.materials[0] = _lightsMatOn;
                }
            }
        }
    }

    public void TurnOffLights(GameObject[] pointLights, GameObject[] prefabLights)
    {
        foreach (var lightsObj in pointLights)
        {
            lightsObj.SetActive(false);
        }

        foreach (var prefabLightsObj in prefabLights)
        {
            foreach (var mr in prefabLightsObj.GetComponentsInChildren<MeshRenderer>())
            {
                if (mr.materials.Length == 1)
                {
                    mr.materials[0] = _lightsMatOff;
                }
            }
        }
    }

    public void TurnOnLightsGlobal()
    {
        foreach(var lightsObj in _lights)
        {
            lightsObj.SetActive(true);
        }

        _lightsMat.EnableKeyword("_EMISSION");
        _lightsMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
    }

    public void TurnOffLightsGlobal()
    {
        foreach (var lightsObj in _lights)
        {
            lightsObj.SetActive(false);
        }


        _lightsMat.DisableKeyword("_EMISSION");
        _lightsMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }

    public void TurnOnAlarm() => _alarms.SetActive(true);
    public void TurnOffAlarm() => _alarms.SetActive(false);
}
