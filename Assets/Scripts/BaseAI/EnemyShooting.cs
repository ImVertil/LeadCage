using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject bullet;
    private float force = 50f;
    private float countdown = 0f;
    private float delay = 1f;
    public bool shoot = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (shoot == true)
        {
            if (countdown <= 0f)
            {
                Instantiate(bullet, bulletOrigin.position, transform.rotation);
                countdown = 1f / delay;
            }
            countdown -= Time.deltaTime;
        }
    }
}
