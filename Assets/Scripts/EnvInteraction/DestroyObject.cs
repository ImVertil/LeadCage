using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float _objectDurability;
    public GameObject broken;
    public GameObject originalObject;
    private GameObject _brokenObj;
    

    void OnMouseDown()
    {
        _objectDurability = _objectDurability - 1;
        if (_objectDurability <= 0)
        {
            BreakObject();
        }
        
    }

    void BreakObject()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);
            if (broken != null)
            {
                _brokenObj = Instantiate(broken, originalObject.transform.position, originalObject.transform.rotation);
                foreach (Transform t in _brokenObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(350f, transform.position, 1f);
                    }
                }
            }
        }
    }

}
