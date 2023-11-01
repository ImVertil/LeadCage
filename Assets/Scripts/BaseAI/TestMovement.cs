using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestMovement : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
        animator.SetFloat("EnemySpeed", agent.velocity.magnitude);
        Debug.Log("EnemySpeed" + agent.velocity.magnitude);
    }
}
