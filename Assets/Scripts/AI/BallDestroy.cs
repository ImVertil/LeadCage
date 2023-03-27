using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroy : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Invoke("DestroyBall", 1f);
    }

    private void DestroyBall()
    {
        Destroy(gameObject);
    }
}
