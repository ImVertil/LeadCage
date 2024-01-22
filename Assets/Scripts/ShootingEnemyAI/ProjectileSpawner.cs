using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileSpawner : MonoBehaviour
{
    private Shooting shooting;
    public ObjectPool<Projectile> _pool;


    private void Start()
    {
        shooting = GetComponent<Shooting>();
        _pool = new ObjectPool<Projectile>(CreateProjectile, OnTakeProjectileFromPool, OnReturnProjectileToPool, OnDestroyProjectile, true, 100, 150);
    }

    private Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(shooting.projectilePrefab, shooting.spawnPoint.position, shooting.spawnPoint.rotation);

        projectile.SetPool(_pool);

        return projectile;
    }

    private void OnTakeProjectileFromPool(Projectile projectile)
    {
        projectile.transform.position = shooting.spawnPoint.position;
        projectile.transform.right = shooting.spawnPoint.right;
        projectile.gameObject.SetActive(true);
    }

    private void OnReturnProjectileToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }
}
