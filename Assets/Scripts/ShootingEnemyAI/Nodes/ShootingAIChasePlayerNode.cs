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
        //ai.SetColor(Color.yellow);
        //Debug.Log("enemy chasing player");
        /*float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance < 1.5f && ai.canSeePlayer)
        {
            Debug.Log("SUCCESS");
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
        else
        {
            Debug.Log("RUNNING");
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }*/

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

        /*if (distance > 0.5f)
        {
            ai.isShooting = false;
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }
        else if (distance < 1.5f && ai.canSeePlayer)
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }*/
    }
}
