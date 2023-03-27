using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;

    public int EnemyHp;

    public LayerMask whatIsPlayer;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public bool isDead;
    public float enemySight;
    public float enemyAttackRange;
    public bool playerInSightRange;
    public bool playerInStandRange;
    public bool isStopped;
    public float distance;

    public GameObject projectile;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (!isDead)
        {
            distance = Vector3.Distance(player.transform.position, transform.position);
            if(distance < enemySight && distance > enemyAttackRange) {
                ChasePlayer();
            } else if (distance <= enemyAttackRange)
            {
                AttackPlayer();
            }
        }
    }


    private void ChasePlayer()
    {
        if (isDead) return;

        agent.destination = player.position;

    }
    private void AttackPlayer()
    {
        if (isDead) return;

        agent.destination = transform.position;
        
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 4f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        if (isDead) return;
        alreadyAttacked = false;
    }
    public void TakeDamage(int damage)
    {
        EnemyHp -= damage;
        if (EnemyHp < 0)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}
