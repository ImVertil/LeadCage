using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;
    private EnemyShooting enemyShooting;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target, EnemyShooting enemyShooting)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        this.enemyShooting = enemyShooting;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        //ai.gameObject.transform.LookAt()
        agent.isStopped = true;
        //ai.SetColor(Color.yellow);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;
        if(ai.isShooting == true)
        {
            enemyShooting.shoot = true;
        }
        else if(ai.isShooting == false)
        {
            enemyShooting.shoot = false;
        }

        return NodeState.RUNNING;
    }

}