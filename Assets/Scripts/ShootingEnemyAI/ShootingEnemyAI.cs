using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootingEnemyAI : MonoBehaviour
{
    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTransform;
    public GameObject playerRef;
    private NavMeshAgent agent;
    private Node topNode;
    Animator animator;
    public bool isShooting;
    Shooting shootingTest;

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

    //Keycard
    [SerializeField] private bool _doesDropItemOnDeath;
    [SerializeField] private GameObject _dropPrefab;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        PuzzleEvents.GeneratorActivated += DisableEnemy;
    }

    private void Start()
    {
        shootingTest = GetComponent<Shooting>();
        playerRef = playerTransform.gameObject;
        UpdateDest();
        StartCoroutine(ShootingFOVRoutine());
        ConstructBehahaviourTree();
        takeAction = false;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Animacje
        if (isShooting)
        {
            animator.SetBool("isShooting", true);
            shootingTest.shoot = true;
        }
        else
        {
            animator.SetBool("isShooting", false);
            shootingTest.shoot = false;
        }

        animator.SetFloat("EnemySpeed", agent.velocity.magnitude);


        //Rozpoczecie ataku w momencie zobaczenia gracza
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
        if (waypoints.Length > 1)
        {
            if (takeAction == false && Vector3.Distance(transform.position, patrolDest) < 2)
            {
                NextWaypoint();
                UpdateDest();

            }
        }
        else if (waypoints.Length == 1)
        {
            if (takeAction == false && Vector3.Distance(transform.position, patrolDest) < 2)
            {
                agent.isStopped = true;
            }
            else if (takeAction == true)
            {
                agent.isStopped = false;
            }
        }


        
    }

    //Drzewo behawioralne
    private void ConstructBehahaviourTree()
    {
        ShootingAIChasePlayerNode chaseNode = new ShootingAIChasePlayerNode(playerTransform, agent, this);
        BasicRangeNode chasingRangeNode = new BasicRangeNode(chasingRange, playerTransform, transform);
        //BasicRangeNode shootingRangeNode = new BasicRangeNode(shootingRange, playerTransform, transform);
        AttackRangeNode shootingRangeNode = new AttackRangeNode(shootingRange, playerTransform, transform, this, obstructionMask);


        ShootingAIAttackNode attackNode = new ShootingAIAttackNode(agent, this, playerTransform);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, attackNode });

        topNode = new Selector(new List<Node> { shootSequence, chaseSequence });
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

        //Debug.Log(canSeePlayer);
    }

    public void DisableEnemy()
    {
        if(_doesDropItemOnDeath && _dropPrefab != null)
        {
            Instantiate(_dropPrefab, gameObject.transform.position, gameObject.transform.rotation);
        }
        gameObject.SetActive(false);
    }
}
