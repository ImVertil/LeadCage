using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float Health;
    [SerializeField] private float lowHealth;
    [SerializeField] private float healingRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;


    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;

    public float radius;
    [Range(0, 360)]
    public float angle;
    //private Material material;

    public EnemyShooting enemyShooting;

    public GameObject playerRef;
    Animator animator;
    Ragdoll ragdoll;

    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public LayerMask groundMask;

    public bool canSeePlayer;
    public bool takeAction;
    public bool isShooting;

    public bool patrol;
    public bool patrolDestSet;
    public Vector3 patrolDest;
    public Transform[] waypoints;
    int waypointIndex;

    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, Health); }
    }

    private void Awake()
    {
        
        patrol = false;
        agent = GetComponent<NavMeshAgent>();
        //material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        enemyShooting = GetComponent<EnemyShooting>();
        UpdateDest();
        //patrol = true;
        takeAction = false;
        StartCoroutine(FOVRoutine());
        _currentHealth = Health;
        ConstructBehahaviourTree();
        animator = GetComponent<Animator>();
    }

    private void ConstructBehahaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealth);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform, obstructionMask);

        ShootRangeNode shootRangeNode = new ShootRangeNode(shootingRange, playerTransform, transform, this, obstructionMask);
        //RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform, enemyShooting);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootRangeNode, shootNode });
        //Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new Selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });


    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

/*    private void Update()
    {
        topNode.Evaluate();
        if (topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healingRate;
    }*/


    private void Update()
    {
        
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            //ragdoll.ActivateRagdoll();
        }
        if (isShooting)
        {
            animator.SetBool("isShooting", true);
            //Debug.Log(isShooting);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }

        animator.SetFloat("EnemySpeed", agent.velocity.magnitude);
        //Debug.Log("enemyHP " + currentHealth);
        float distanceToTarget = Vector3.Distance(transform.position, playerRef.transform.position);
//        Debug.Log(distanceToTarget);
        if (canSeePlayer)
        {
            //patrol = false;
            takeAction = true;
        }

        if (takeAction)
        {
            topNode.Evaluate();
            if (topNode.nodeState == NodeState.FAILURE)
            {
                //SetColor(Color.red);
                UpdateDest();
                //agent.isStopped = true;
                //patrol = true;
            }
        }



        if (distanceToTarget > chasingRange)
        {
            takeAction = false;
            //Debug.Log("dst = " + patrol);
        }

        /*if (takeAction == false)
        {
            //UpdateDest();
            //Debug.Log("UpdateDest ffs");
            if (Vector3.Distance(transform.position,patrolDest.normalized) < 1)
            {
                Debug.Log("next ffs");
                NextWaypoint();
                UpdateDest();
            }
        }*/

        //Debug.Log(Vector3.Distance(transform.position, patrolDest));
        //Debug.Log("tra   " + transform.position);
        //Debug.Log("patrol     " + patrolDest);


        if (takeAction == false && Vector3.Distance(transform.position, patrolDest) < 2)
        {
            //UpdateDest();
            //Debug.Log("UpdateDest ffs");
            
            
            //Debug.Log("next ffs");
            NextWaypoint();
            UpdateDest();
            
        }

        //Debug.Log(takeAction);


        /*if (patrol)
        {
            StartCoroutine(PatrolRoutine());
        }
        else
        {
            StopCoroutine(PatrolRoutine());
        }

        if (currentHealth < lowHealth)
        {
            patrol = false;
        }*/



        //currentHealth += Time.deltaTime * healingRate;
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;

        if (canSeePlayer)
        {
            //Debug.Log("+++I CAN SEE+++");
            //material.color = Color.green;
        }
        else
        {
            //Debug.Log("---I CAN'T SEE---");
            //material.color = Color.red;
        }
    }

    private void UpdateDest()
    {
        patrolDest = waypoints[waypointIndex].position;
        agent.SetDestination(patrolDest);
    }

    private void NextWaypoint()
    {
        waypointIndex++;
        if(waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

/*    public void RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        
        patrolDest =  finalPosition;
        patrolDestSet = true;
    }*/

    private void NewPosition()
    {
        float agentX = transform.position.x;
        float agentZ = transform.position.z;

        float randomX = Random.Range(agentX + 5f, agentX - 5);
        float randomZ = Random.Range(agentZ + 5f, agentZ - 5);

        //Debug.Log("new possssssssss");

        patrolDest = new Vector3(transform.position.x+randomX, transform.position.y, transform.position.z + randomZ);
        //Debug.Log("patroldest " + patrolDest); 
        if (Physics.Raycast(patrolDest, -transform.up, 2f, groundMask))
        {
            //Debug.Log("iffffffff");
            patrolDestSet = true;
        }
    }

    private void Patrol()
    {
        if (!patrolDestSet)
        {
            //Debug.Log("new dest");
            //RandomNavmeshLocation(10f);
            NewPosition(); 
        }

        if (patrolDestSet)
        {
            agent.SetDestination(patrolDest);
        }
        Vector3 distanceToPD = transform.position - patrolDest;

        if (distanceToPD.magnitude < 1)
        {
            //Debug.Log("arrived");
            patrolDestSet = false;
        }


        /*if (patrolDestSet)
        {
            Debug.Log("set dest");
            agent.SetDestination(patrolDest);
            Vector3 distanceToPD = transform.position - patrolDest;

            if (distanceToPD.magnitude < 1)
            {
                Debug.Log("arrived");
                patrolDestSet = false;
            }
        }*/

        
    }

    /*private IEnumerator PatrolRoutine()
    {
        WaitForSeconds wait2 = new WaitForSeconds(2f);


        while (true)
        {
            Vector3 patrolLocation = RandomNavmeshLocation(100f);
            agent.SetDestination(patrolLocation);
            Debug.Log("COR");
            yield return wait2;
            
        }
    }*/


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log(currentHealth);
    }

    /*public void SetColor(Color color)
    {
        material.color = color;
    }*/

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }


}