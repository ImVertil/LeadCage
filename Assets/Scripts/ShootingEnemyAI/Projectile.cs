using System.Collections;
using UnityEngine;
using UnityEngine.Pool;


public class Projectile : MonoBehaviour
{
    private Rigidbody _rb;
    private ObjectPool<Projectile> _pool;
    [SerializeField] private float _destroyTime = 2f;

    //private Coroutine deactivateProjectileAfterTimeCoroutine;
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
        //deactivateProjectileAfterTimeCoroutine = StartCoroutine(DeactivateProjectileAfterTime());
        _mr.enabled = true;
        _collider.enabled = true;
        StartCoroutine(DeactivateProjectileAfterTime());
        _rb.velocity = transform.right * 20f;
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
        //_pool.Release(this);
        _mr.enabled = false;
        _collider.enabled = false;
    }

    

    private IEnumerator DeactivateProjectileAfterTime()
    {
        //float timePassed = 0f;
        /*while(timePassed < _destroyTime)
        {
            timePassed += Time.deltaTime;
            yield return null;
        }*/
        yield return new WaitForSeconds(_destroyTime);
        _pool.Release(this);
    }


    /*public void Init(Vector3 direction, ObjectPool<Projectile> pool)
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

    private void Destroy() => _pool.Release(this);*/
}
