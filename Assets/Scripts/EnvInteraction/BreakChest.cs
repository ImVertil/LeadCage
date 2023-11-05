using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakChest : MonoBehaviour
{
    public GameObject brokenChest;
    private GameObject _brokenObj;
    private GameObject _heavyobject;
    public LayerMask heavierLayer;

    private void OnCollisionEnter(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (heavierLayer == (heavierLayer | (1 << layer)))
        {
            _heavyobject = collision.gameObject;
            gameObject.SetActive(false);

            Quaternion rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            _brokenObj = Instantiate(brokenChest, transform.position, rotation);
            foreach (Transform t in _brokenObj.transform)
            {
                var rb = t.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(350f, transform.position, 1f);
                }
            }
            Invoke("Deactivate", 5f);
        }
    }

    private void Deactivate()
    {
        if (_brokenObj != null)
        {
            _brokenObj.SetActive(false);
        }
        if (_heavyobject != null)
        {
            _heavyobject.SetActive(false);
        }
    }

/*    private void OnMouseDown()
    {
        gameObject.SetActive(false);

        Quaternion rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
        _brokenObj = Instantiate(brokenChest, transform.position, rotation);
        foreach (Transform t in _brokenObj.transform)
        {
            var rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(350f, transform.position, 1f);
            }
        }
    }*/
}
