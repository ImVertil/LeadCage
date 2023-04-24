using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeNode : Node
{
    private float range;
    private Transform target;
    private Transform origin;
    LayerMask obstruction;

    public RangeNode(float range, Transform target, Transform origin, LayerMask obstruction)
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



        /*if(distance <= range)
        {
            
            return NodeState.SUCCESS;
        } else if (distance > range && !Physics.Raycast(origin.position, directionToTarget, distance, obstruction))
        {
            return NodeState.SUCCESS;
        }
        else
        {
            
            return NodeState.FAILURE;
        }*/
    }
}
