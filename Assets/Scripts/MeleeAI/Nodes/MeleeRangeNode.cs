using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;

    public MeleeRangeNode(float range, Transform target, Transform origin)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}


















/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    LayerMask obstruction;

    public MeleeRangeNode(float range, Transform target, Transform origin, LayerMask obstruction)
    {
        this.range = range;
        this.target = target;
        this.origin = origin;
        this.obstruction = obstruction;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, origin.position);
        Vector3 directionToTarget = (target.position - origin.position).normalized;


        return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;



        if (distance <= range)
        {

            return NodeState.SUCCESS;
        }
        else if (distance > range && !Physics.Raycast(origin.position, directionToTarget, distance, obstruction))
        {
            return NodeState.SUCCESS;
        }
        else
        {

            return NodeState.FAILURE;
        }
    }
}
*/