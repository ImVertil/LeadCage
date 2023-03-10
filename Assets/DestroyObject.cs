using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float objectDurability;
    public GameObject broken;
    public GameObject originalObject;
    private GameObject brokenObj;
    

    void OnMouseDown()
    {
        objectDurability = objectDurability - 1;
        if (objectDurability <= 0)
        {
            Break();
        }
        
    }

    void Break()
    {
        if (originalObject != null)
        {
            originalObject.SetActive(false);
            if (broken != null)
            {
                brokenObj = Instantiate(broken, originalObject.transform.position, originalObject.transform.rotation) as GameObject;
                foreach (Transform t in brokenObj.transform)
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
