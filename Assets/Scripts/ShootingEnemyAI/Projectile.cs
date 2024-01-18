using UnityEngine;
using UnityEngine.Pool;


public class Projectile : MonoBehaviour
{
    private ObjectPool<Projectile> _pool;

    public void Init(Vector3 direction, ObjectPool<Projectile> pool)
    {
        transform.forward = direction;
        _pool = pool;
        gameObject.SetActive(true);
        
    }

    private void Update()
    {
        transform.position += transform.forward * 10 * Time.deltaTime;
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
            
        }
        Destroy();
    }

    private void Destroy() => _pool.Release(this);
}
