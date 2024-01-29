using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    [SerializeField] public Projectile projectilePrefab;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public bool shoot = true;
    [SerializeField] private float _firerate = 10f;
    private float _timer;
    private Transform _enemyTransform;
    private ProjectileSpawner projectileSpawner;

    private void Start()
    {
        projectileSpawner = GetComponent<ProjectileSpawner>();
    }

    private void Update()
    {
        if (shoot)
        {
            _timer += Time.deltaTime;
            if (_timer >= 2f / _firerate)
            {
                projectileSpawner._pool.Get();
                _timer = 0f;
            }
            
        }
    }
}
