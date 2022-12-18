using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementController : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField]
    private Transform postoGO;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    private void Start()
    {
        agent.SetDestination(postoGO.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
