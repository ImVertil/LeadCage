using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeChaseNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private MeleeAI ai;

    public MeleeChaseNode(Transform target, NavMeshAgent agent, MeleeAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance > 0.2f)
        {
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



















/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeChaseNode : Node
{
    private Transform target;
    private NavMeshAgent agent;
    private MeleeAI ai;

    public MeleeChaseNode(Transform target, NavMeshAgent agent, MeleeAI ai)
    {
        this.target = target;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.gray);
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance < 0.2f && ai.canSeePlayer)
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }



        if (distance > 0.2f)
        {
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


}*/