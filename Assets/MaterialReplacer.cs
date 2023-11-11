using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MaterialReplacer : MonoBehaviour
{
    [SerializeField] private Material _oldMat;
    [SerializeField] private Material _newMat;
    [SerializeField] private GameObject _objects;

    [SerializeField]
    private void Start()
    {
        foreach (var mr in _objects.GetComponentsInChildren<MeshRenderer>())
        {
            /*Material[] cachedMats = new Material[mr.materials.Length];
            for (int i = 0; i < mr.materials.Length; i++)
            {
                if (mr.materials[i].name == _oldMat.name)
                {
                    Debug.Log("yes");
                    cachedMats[i] = _newMat;
                }
                else
                {
                    Debug.Log("no");
                    cachedMats[i] = mr.materials[i];
                }
            }
            mr.materials = cachedMats;*/
            for (int i=0; i<mr.sharedMaterials.Length; i++)
            {
                if (mr.sharedMaterials[i].name == _oldMat.name)
                {
                    mr.sharedMaterials[i] = _newMat;
                    Debug.Log("yes");
                }
                else
                {
                    Debug.Log("no");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
