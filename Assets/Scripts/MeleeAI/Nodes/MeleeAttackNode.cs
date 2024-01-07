using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private NavMeshAgent agent;
    private MeleeAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public MeleeAttackNode(NavMeshAgent agent, MeleeAI ai, Transform target)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        //ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;
        return NodeState.RUNNING;
    }

}




















/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAttackNode : Node
{
    private NavMeshAgent agent;
    private MeleeAI ai;
    private Transform target;
    private MeleeAttack meleeAttack;

    private Vector3 currentVelocity;
    private float smoothDamp;

    public MeleeAttackNode(NavMeshAgent agent, MeleeAI ai, Transform target, MeleeAttack meleeAttack)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        this.meleeAttack = meleeAttack;
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
        if (ai.isShooting == true)
        {
            
            //enemyShooting.shoot = true;
        }
        else if (ai.isShooting == false)
        {
            //enemyShooting.shoot = false;
        }

        return NodeState.RUNNING;
    }

}*/