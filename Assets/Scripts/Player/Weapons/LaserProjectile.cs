using System.Collections;
using UnityEngine;
using UnityEngine.Pool;



class LaserProjectile : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    [SerializeField] private float _destroyTime = 2f;
    [SerializeField] private float _velocity = 20f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateProjectileAfterTime());
        _rb.velocity = transform.forward * _velocity;
    }
    
    private IEnumerator DeactivateProjectileAfterTime()
    {
        yield return new WaitForSeconds(_destroyTime);
        this.gameObject.SetActive(false);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        this.gameObject.SetActive(false);
    }
}
