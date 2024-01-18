using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingAIAttackNode : Node
{
    private NavMeshAgent agent;
    private ShootingEnemyAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public ShootingAIAttackNode(NavMeshAgent agent, ShootingEnemyAI ai, Transform target)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.isShooting = true;
        //ai.SetColor(Color.green);
        //Debug.Log("enemy shooting player");
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;
        return NodeState.RUNNING;
    }
}
