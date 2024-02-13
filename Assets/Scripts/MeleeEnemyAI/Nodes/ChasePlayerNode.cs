using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayerNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private MeleeEnemyAI ai;

    public ChasePlayerNode(Transform target, NavMeshAgent agent, MeleeEnemyAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance > 0.5f)
        {
            ai.isShooting = false;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
