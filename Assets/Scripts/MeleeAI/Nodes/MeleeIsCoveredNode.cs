using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeIsCoveredNode : Node
{
    private Transform target;
    private Transform origin;

    public MeleeIsCoveredNode(Transform target, Transform origin)
    {
        this.target = target;
        this.origin = origin;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, target.position - origin.position, out hit))
        {
            if (hit.collider.transform != target)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;
    }

    //dodac skrypt healthAI ktory ma metode HEAL() i ta metoda uruchamia siê w momencie kiedy ten node ma wartosc success
}