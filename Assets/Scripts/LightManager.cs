using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance;

    [SerializeField] private Material _alarmMatOn;
    [SerializeField] private Material _alarmMatOff;
    [SerializeField] private Material _lightsMatOn;
    [SerializeField] private Material _lightsMatOff;
    [SerializeField] private GameObject[] _alarms;
    [SerializeField] private GameObject[] _lights;

    [SerializeField] private GameObject[] _lightPrefabs;

    private List<MeshRenderer> _meshRenderers = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;

        foreach(var prefab in _lightPrefabs)
        {
            foreach(var mr in prefab.GetComponentsInChildren<MeshRenderer>())
            {
                _meshRenderers.Add(mr);
            }
        }
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
                if (mr.sharedMaterials.Length == 1 && mr.sharedMaterial.name == "Decals_2_off")
                {
                    mr.sharedMaterial = _lightsMatOn;
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
                if (mr.sharedMaterials.Length == 1 && mr.sharedMaterial.name == "Decals_2")
                {
                    mr.sharedMaterial = _lightsMatOff;
                }
            }
        }
    }

    public void TurnOnLightsGlobal()
    {
        foreach (var lightsObj in _lights)
        {
            lightsObj.SetActive(true);
        }

        Material[] materialsCopy;
        foreach (var prefabLightsObj in _lightPrefabs)
        {
            foreach (var mr in prefabLightsObj.GetComponentsInChildren<MeshRenderer>())
            {
                int length = mr.sharedMaterials.Length;
                if (length > 1)
                {
                    if (mr.sharedMaterials[length - 1].name == "Decals_2_off") // decals always at the end of the array
                    {
                        // copy the array, otherwise can't modify one of the sharedMaterials if multiple.
                        materialsCopy = mr.sharedMaterials;
                        materialsCopy[length - 1] = _lightsMatOn;
                        mr.sharedMaterials = materialsCopy;
                    }
                }
                else if (mr.sharedMaterial.name == "Decals_2_off")
                {
                    mr.sharedMaterial = _lightsMatOn;
                }
            }
        }

        //_lightsMat.EnableKeyword("_EMISSION");
        // _lightsMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
    }

    public void TurnOffLightsGlobal()
    {
        foreach (var lightsObj in _lights)
        {
            lightsObj.SetActive(false);
        }

        Material[] materialsCopy;
        foreach (var prefabLightsObj in _lightPrefabs)
        {
            foreach (var mr in prefabLightsObj.GetComponentsInChildren<MeshRenderer>())
            {
                int length = mr.sharedMaterials.Length;
                if(length > 1)
                {
                    if (mr.sharedMaterials[length - 1].name == "Decals_2") // decalsy zawsze na koñcu s¹
                    {
                        materialsCopy = mr.sharedMaterials;
                        materialsCopy[length - 1] = _lightsMatOff;
                        mr.sharedMaterials = materialsCopy;
                    }
                }
                else if (mr.sharedMaterial.name == "Decals_2")
                {
                    mr.sharedMaterial = _lightsMatOff;
                }
            }
        }
        //_lightsMat.DisableKeyword("_EMISSION");
        //_lightsMat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;
    }

    public void TurnOnAlarm()
    {
        foreach (var alarmObj in _alarms)
        {
            foreach (var alarmLight in alarmObj.GetComponentsInChildren<Light>())
            {
                alarmLight.GetComponentInChildren<MeshRenderer>().material = _alarmMatOn;
                alarmLight.enabled = true;
            }

            foreach (var alarmSound in alarmObj.GetComponentsInChildren<AudioSource>())
            {
                alarmSound.enabled = true;
                alarmSound.gameObject.GetComponentInChildren<Animator>().enabled = true;
            }
        }
    }
    public void TurnOffAlarm()
    {
        foreach(var alarmObj in _alarms)
        {
            foreach (var alarmLight in alarmObj.GetComponentsInChildren<Light>())
            {
                alarmLight.GetComponentInChildren<MeshRenderer>().material = _alarmMatOff;
                alarmLight.enabled = false;
            }

            foreach (var alarmSound in alarmObj.GetComponentsInChildren<AudioSource>())
            {
                alarmSound.enabled = false;
                alarmSound.gameObject.GetComponentInChildren<Animator>().enabled = false;
            }
        }
    }
}
