using System.Collections;
using UnityEngine;
using UnityEngine.Pool;


public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private ObjectPool<Projectile> _pool;
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] private float _velocity = 20f;
    private Collider _collider;
    private MeshRenderer _mr;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _mr = GetComponent<MeshRenderer>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        _mr.enabled = true;
        _collider.enabled = true;
        StartCoroutine(DeactivateProjectileAfterTime());
        _rb.velocity = transform.right * _velocity;
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        _pool = pool;
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
        _mr.enabled = false;
        _collider.enabled = false;
    }

    

    private IEnumerator DeactivateProjectileAfterTime()
    {
        yield return new WaitForSeconds(_destroyTime);
        _pool.Release(this);
    }
}
