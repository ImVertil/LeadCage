using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTransform;
    public GameObject playerRef;
    private NavMeshAgent agent;
    private Node topNode;
    Animator animator;
    public bool isShooting;

    //FOV
    public bool takeAction;
    public bool canSeePlayer;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public float radius;
    [Range(0, 360)]
    public float angle;


    //Patrol
    public bool patrolDestSet;
    public Vector3 patrolDest;
    public Transform[] waypoints;
    int waypointIndex;

    private float _currentHealth;
    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerRef = playerTransform.gameObject;
        UpdateDest();
        StartCoroutine(ShootingFOVRoutine());
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
        takeAction = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Animacje
        if (isShooting)
        {
            animator.SetBool("isAttacking", true);
            //Debug.Log(isShooting);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        animator.SetFloat("EnemySpeed", agent.velocity.magnitude);


        //Rozpoczecie ataku w momencie zobaczenia gracza(jesli bedzie czas, zamienic na maszyne stanowa)
        if (canSeePlayer)
        {
            takeAction = true;
        }

        if (takeAction)
        {
            topNode.Evaluate();
            if (topNode.nodeState == NodeState.FAILURE)
            {
                UpdateDest();
            }
        }

        //Sprawdzanie czy gracz uciekl
        float distanceToTarget = Vector3.Distance(transform.position, playerRef.transform.position);
        if (distanceToTarget > chasingRange)
        {
            takeAction = false;
        }


        //Przejscie do kolejnego punktu patrolu
        if (takeAction == false && Vector3.Distance(transform.position, patrolDest) < 2)
        {
            NextWaypoint();
            UpdateDest();

        }
    }
    //AttackRangeNode shootingRangeNode = new AttackRangeNode(shootingRange, playerTransform, transform, this);
    //Drzewo behawioralne
    private void ConstructBehahaviourTree()
    {
        ChasePlayerNode chaseNode = new ChasePlayerNode(playerTransform, agent, this);
        BasicRangeNode chasingRangeNode = new BasicRangeNode(chasingRange, playerTransform, transform);
        BasicRangeNode shootingRangeNode = new BasicRangeNode(shootingRange, playerTransform, transform);

        
        AttackNode attackNode = new AttackNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, attackNode });

        topNode = new Selector(new List<Node> {shootSequence, chaseSequence });
    }

    //Patrol
    private void UpdateDest()
    {
        patrolDest = waypoints[waypointIndex].position;
        agent.SetDestination(patrolDest);
    }

    private void NextWaypoint()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }


    //Sprawdzanie czy wrog widzi gracza
    private IEnumerator ShootingFOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
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
            Debug.Log("+++I CAN SEE+++");
        }
        else
        {
            Debug.Log("---I CAN'T SEE---");
        }
    }

}
