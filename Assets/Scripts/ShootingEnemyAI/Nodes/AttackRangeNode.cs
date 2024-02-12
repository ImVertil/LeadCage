using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    private ShootingEnemyAI ai;
    LayerMask obstruction;

    public AttackRangeNode(float range, Transform target, Transform origin, ShootingEnemyAI ai, LayerMask obstruction)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
        this.ai = ai;
        this.obstruction = obstruction;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        Vector3 directionToTarget = (target.position - origin.position).normalized;
        if (distance <= range && !Physics.Raycast(origin.position, directionToTarget, distance, obstruction))
        {
            ai.isShooting = true;
            return NodeState.SUCCESS;
        }
        else
        {
            ai.isShooting = false;
            return NodeState.FAILURE;
        }
    }
}
