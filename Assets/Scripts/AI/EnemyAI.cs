/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] public Transform _player;
    public GameObject bullet;

    
    public int EnemyHp;
    public float _delayAttack;
    private float _distanceFromPlayer;
    public float enemySight;
    public float enemyAttackRange;
    private bool _isDead;
    bool attacked;

    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!_isDead)
        {
            _distanceFromPlayer = Vector3.Distance(_player.transform.position, transform.position);
            if(_distanceFromPlayer < enemySight && _distanceFromPlayer > enemyAttackRange) {
                runAfterPlayer();
            } else if (_distanceFromPlayer <= enemyAttackRange)
            {
                Attack();
            }
        }
    }

    private void runAfterPlayer()
    {
        if (_isDead) return;
        agent.destination = _player.position;
    }
    private void Attack()
    {
        if (_isDead) return;
        agent.destination = transform.position;
        transform.LookAt(_player);
        if (!attacked)
        {
            Rigidbody rigidBody = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rigidBody.AddForce(transform.forward * 4f, ForceMode.Impulse);
            attacked = true;
            Invoke("Reload", _delayAttack);
        }
    }
    private void Reload()
    {
        if (_isDead) return;
        attacked = false;
    }
    public void TakeDamage(int damage)
    {
        EnemyHp -= damage;
        if (EnemyHp < 0)
        {
            _isDead = true;
            Destroy(gameObject);
        }
    }
}*/