using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingAIChasePlayerNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private ShootingEnemyAI ai;

    public ShootingAIChasePlayerNode(Transform target, NavMeshAgent agent, ShootingEnemyAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance < 0.5f && ai.canSeePlayer)
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
        else
        {
            ai.isShooting = false;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }
    }
}
