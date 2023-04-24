using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDestination : MonoBehaviour
{
    private NavMeshAgent player;
    [SerializeField] private Transform dest;

    private void Awake()
    {
        player = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        player.destination = dest.position;
    }
}
