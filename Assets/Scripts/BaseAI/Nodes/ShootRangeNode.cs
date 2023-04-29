using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    private EnemyAI ai;
    LayerMask obstruction;

    public ShootRangeNode(float range, Transform target, Transform origin, EnemyAI ai, LayerMask obstruction)
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
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.FAILURE;
        }


        /*        if (distance <= range && ai.canSeePlayer)
                {
                    return NodeState.SUCCESS;
                }
                else if (distance <= range && !ai.canSeePlayer)
                {
                    return NodeState.FAILURE;
                } else if (distance > range && !ai.canSeePlayer)
                {
                    return NodeState.FAILURE;
                } else if (distance > range && ai.canSeePlayer)
                {
                    return NodeState.FAILURE;
                }*/

    }
}
