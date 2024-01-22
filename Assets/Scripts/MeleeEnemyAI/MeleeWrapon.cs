using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWrapon : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.gameObject.layer == 8)
        {
            Debug.Log(collision.gameObject.tag + " + " + collision.gameObject.name);
            Health playerH = collision.transform.GetComponent<Health>();
            if (playerH != null)
            {
                playerH.TakeDamage(10f);
            }
        }
    }
}
