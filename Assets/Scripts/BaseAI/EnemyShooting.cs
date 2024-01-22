/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
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
            Debug.Log("shooting at   " +  xRotation + " " + yRotation);
            Debug.Log("fireCountdown   " + fireCountdown);
        }
    }
}*/




/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private GameObject bullet;
    private float force = 50f;

    private float fireCountdown = 0f;
    private float bulletDelay = 0.3f;

    private float sprayCountdown = 0f;
    private float delay = 1f;
    public bool shoot = false;




    *//*    void Update()
        {
            if (shoot == true)
            {
                if (sprayCountdown <= 0f)
                {
                    if (fireCountdown <= 0f)
                    {
                        Instantiate(bullet, bulletOrigin.position, transform.rotation);
                        sprayCountdown = 1f / delay;
                    }
                    fireCountdown -= Time.deltaTime;

                    Quaternion secondRotation = transform.rotation * Quaternion.Euler(-5f, 2f, 0f);
                    if (fireCountdown <= 0f)
                    {
                        Instantiate(bullet, bulletOrigin.position, secondRotation);
                        sprayCountdown = 1f / delay;
                    }
                    fireCountdown -= Time.deltaTime;

                    Quaternion thirdRotation = transform.rotation * Quaternion.Euler(-10f, 4f, 0f);
                    if (fireCountdown <= 0f)
                    {
                        Instantiate(bullet, bulletOrigin.position, thirdRotation);
                        sprayCountdown = 1f / delay;
                    }
                    fireCountdown -= Time.deltaTime;
                }
                sprayCountdown -= Time.deltaTime;
            }
        }*//*


    //OSTATNIA WER

    *//*    void Update()
        {
            if (shoot == true)
            {
                if (sprayCountdown <= 0f)
                {
                    FireBullet();
                    sprayCountdown = 1f / delay;
                }
                sprayCountdown -= Time.deltaTime;
            }
        }

        void FireBullet()
        {
            float rotationX = 4f;
            float rotationY = -2f;

            Quaternion rotation = transform.rotation * Quaternion.Euler(rotationX, rotationY, 0f);
            for (int i = 0; i < 4; i++)
            {
                Instantiate(bullet, bulletOrigin.position, rotation);
                rotationX += 4f;
                rotationY -= 2f;
            }

        }*//*

    BulletObjectPooling bulletObjectPooling;

    private void Start()
    {
        bulletObjectPooling = BulletObjectPooling.Instance;
    }

    private void FixedUpdate()
    {
        if (shoot == true)
        {
            float rotationX = 4f;
            float rotationY = -2f;

            Quaternion rotation = transform.rotation * Quaternion.Euler(rotationX, rotationY, 0f);
            for (int i = 0; i < 4; i++)
            {
                //Instantiate(bullet, bulletOrigin.position, rotation);
                if (i < 4)
                {
                    rotationX += 4f;
                    rotationY -= 2f;
                }
                else
                {
                    rotationX = 4f;
                    rotationY = -2f;
                }

            }

            bulletObjectPooling.SpawnFromPool("bullet", bulletOrigin.position, rotation);
        }
    }
}*/





using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Bullet _bullet;
    [SerializeField] private int _bulletsSpawned;
    [SerializeField] private bool _usepool;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxCapacity = 30;
    private ObjectPool<Bullet> _pool;


    public bool shoot = false;

    private void Start()
    {


        _pool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bullet);
        }, bullet =>
        {
            bullet.gameObject.SetActive(true);
        }, bullet =>
        {
            bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet.gameObject);
        }, false, 10, 20);

        InvokeRepeating(nameof(Shoot), 0.5f, 0.5f);
    }



    private void Shoot()
    {
        if (shoot == true)
        {
            for (var i = 0; i < _bulletsSpawned; i++)
            {
                //pobieranie z puli lub generowanie
                //var bullet = _usepool ? _pool.Get() : Instantiate(_bullet);
                var bullet = _pool.Get();
                bullet.transform.position = bulletOrigin.position;

                //rotacja
                float rotationX = UnityEngine.Random.Range(-5f, 5f);
                float rotationY = UnityEngine.Random.Range(-2f, 2f);
                Quaternion rotation = transform.rotation * Quaternion.Euler(rotationX, rotationY, 0f);
                bullet.transform.rotation = rotation;

                //force
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                bulletRigidbody.velocity = bullet.transform.forward * 15f;

                bullet.Init(KillBullet);

            }
        }
    }

    private void KillBullet(Bullet bullet)
    {
        _pool.Release(bullet);
        /*if(_usepool == true)
        {
            _pool.Release(bullet);
        } else
        {
            Destroy(bullet.gameObject);
        }*/
    }
}