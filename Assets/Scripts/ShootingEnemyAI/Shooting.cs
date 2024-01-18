using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _spawnPoint;
    private float _firerate;
    private float _timer;
    private Transform _enemyTransform;
    private ObjectPool<Projectile> _pool;


    private void Awake()
    {
        _enemyTransform = gameObject.transform;
        _pool = new ObjectPool<Projectile>(CreateProjectile, null, OnPutBackInPool, defaultCapacity: 500);
    }

    private void OnPutBackInPool(Projectile obj)
    {
        obj.gameObject.SetActive(false);
    }

    private Projectile CreateProjectile()
    {
        var projectile = Instantiate(_projectilePrefab);
        return projectile;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 2f / _firerate)
        {
            Shoot();
            _timer = 0f;
        }
    }

    private void SpawnProjectile()
    {
        var projectile = _pool.Get();
        projectile.transform.position = _spawnPoint.transform.position;
        projectile.Init(_spawnPoint.transform.position, _pool);
    }

    private void Shoot()
    {
        SpawnProjectile();

    }


}
