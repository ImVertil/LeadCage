using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    private MeleeEnemyAI ai;

    public AttackRangeNode(float range, Transform target, Transform origin, MeleeEnemyAI ai)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        Vector3 directionToTarget = (target.position - origin.position).normalized;

        if (distance <= range && !Physics.Raycast(origin.position, directionToTarget, distance))
        {
            ai.isShooting = true;
            Debug.Log("AttackRangeNode SUCCESS");

            return NodeState.SUCCESS;
        }
        else
        {
            ai.isShooting = false;
            Debug.Log("AttackRangeNode FAILURE");

            return NodeState.FAILURE;
        }
    }
}
