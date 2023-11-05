using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAI : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float lowHealth;
    [SerializeField] private float healingRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float attackingRange;

    [SerializeField] private Transform playerTransform;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public Transform placeToGuard;
    private UnityEngine.AI.NavMeshAgent agent;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public LayerMask groundMask;


    private void ConstructBehahaviourTree()
    {
        //ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        //RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform, obstructionMask);
    }
}
