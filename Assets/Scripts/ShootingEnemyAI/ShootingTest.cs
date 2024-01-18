using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTest : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject bullet;
    private float force = 50f;

    private float fireCountdown = 0f;
    private float bulletDelay = 0.3f;

    private float sprayCountdown = 0f;
    private float delay = 1f;
    public bool shoot = false;

    void Update()
    {
        if (shoot)
        {
            sprayCountdown -= Time.deltaTime;

            if (sprayCountdown <= 0f)
            {
                for (int i = 0; i < 3; i++)
                {
                    FireBullet(-5f * i, 2f * i);
                }

                sprayCountdown = delay;
            }
        }
    }

    void FireBullet(float xRotation, float yRotation)
    {
        fireCountdown -= Time.deltaTime;

        if (fireCountdown <= 0f)
        {
            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            Instantiate(bullet, bulletOrigin.position, rotation);
            fireCountdown = bulletDelay;
            Debug.Log("shooting at   " + xRotation + " " + yRotation);
            Debug.Log("fireCountdown   " + fireCountdown);
        }
    }
}