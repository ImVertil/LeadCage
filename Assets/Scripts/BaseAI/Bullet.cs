using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force;
    void Start()
    {
        rigidbody.velocity = transform.forward * force;
    }

    void Update()
    {

        Destroy(gameObject,5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Health playerH = collision.transform.GetComponent<Health>();

            if (playerH == null)
            {
                Debug.Log("No hp script");
                return;
            }

            playerH.TakeDamage(10f);
        }

        
    }
}
