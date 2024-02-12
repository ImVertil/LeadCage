using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAI : MonoBehaviour
{
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
    [SerializeField] public Transform lookAtPoint;
    private Vector3 currentVelocity;

    public bool patrolDestSet;
    public Vector3 patrolDest;
    public Transform[] waypoints;
    int waypointIndex;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        playerRef = playerTransform.gameObject;
        UpdateDest();
        StartCoroutine(MeleegFOVRoutine());
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


        //Przejscie do punktu patrolu
        if (takeAction == false)
        {
            UpdateDest();

        }

        if (takeAction == false && Vector3.Distance(transform.position, patrolDest) < 2)
        {
            agent.isStopped = true;

            //patrzenie w miejsce obserwacji
            Vector3 direction = lookAtPoint.position - transform.position;
            Vector3 currentDirection = Vector3.SmoothDamp(transform.forward, direction, ref currentVelocity, 1f);
            Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
            transform.rotation = rotation;

        }
    }
    //Drzewo behawioralne
    private void ConstructBehahaviourTree()
    {
        ChasePlayerNode chaseNode = new ChasePlayerNode(playerTransform, agent, this);
        BasicRangeNode chasingRangeNode = new BasicRangeNode(chasingRange, playerTransform, transform);
        BasicRangeNode attackRangeNode = new BasicRangeNode(shootingRange, playerTransform, transform);

        
        AttackNode attackNode = new AttackNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { attackRangeNode, attackNode });

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
    private IEnumerator MeleegFOVRoutine()
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
    }

}
