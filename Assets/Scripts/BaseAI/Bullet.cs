using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force;

    private Action<Bullet> _killAction;
    public void OnObjectSpawn()
    {
        rigidbody.velocity = transform.forward * force;
    }

    void Update()
    {

        //Destroy(gameObject,5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.gameObject.layer == 8)
        {
            Health playerH = collision.transform.GetComponent<Health>();
            if (playerH != null)
            {
                playerH.TakeDamage(10f);
            }
            _killAction(this);
        }




        /*if (collision.gameObject.tag == "Player")
        {
            Health playerH = collision.transform.GetComponent<Health>();

            if (playerH == null)
            {
                Debug.Log("No hp script");
                return;
            }

            playerH.TakeDamage(10f);
            //Destroy(gameObject);

            _killAction(this);
        }

        if (collision.gameObject.layer == 8)
        {
            //Destroy(gameObject);
            _killAction(this);
        }*/
    }

    public void Init(Action<Bullet> killAction)
    {
        _killAction = killAction;
    }
}
