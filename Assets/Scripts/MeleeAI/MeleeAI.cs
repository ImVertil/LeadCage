using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;



    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
    }

    private void ConstructBehahaviourTree()
    {
        //   IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        //   GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        //  HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        //  IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        MeleeChaseNode chaseNode = new MeleeChaseNode(playerTransform, agent, this);
        MeleeRangeNode chasingRangeNode = new MeleeRangeNode(chasingRange, playerTransform, transform);
        MeleeRangeNode shootingRangeNode = new MeleeRangeNode(shootingRange, playerTransform, transform);
        MeleeAttackNode shootNode = new MeleeAttackNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

       // Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
       // Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
       // Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        //Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Selector(new List<Node> { shootSequence, chaseSequence });


    }

    private void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
        {
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healthRestoreRate;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }



    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }


}














/*using System.Collections;
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
    MeleeAttack meleeAttack;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public LayerMask groundMask;


    private void ConstructBehahaviourTree()
    {
        //ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        //RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform, obstructionMask);


        //HealthNode healthNode = new HealthNode(this, lowHealth);

        MeleeChaseNode chaseNode = new MeleeChaseNode(playerTransform, agent, this);
        MeleeRangeNode chasingRangeNode = new MeleeRangeNode(chasingRange, playerTransform, transform, obstructionMask);
        MeleeRangeNode shootingRangeNode = new MeleeRangeNode(attackingRange, playerTransform, transform, obstructionMask);
        MeleeAttackNode shootNode = new MeleeAttackNode(agent, this, playerTransform, meleeAttack);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });


        topNode = new Selector(new List<Node> { shootSequence, chaseSequence });
    }
}
*/