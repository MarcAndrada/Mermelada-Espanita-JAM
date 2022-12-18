using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourController : MonoBehaviour
{
    public enum EnemyState { WAITING, PATROLING, CHASING, ATTACKING, DEAD };
    public EnemyState currentState;

    [Header("Detection Variables"), SerializeField]
    private float maxVisionDistance;
    [SerializeField]
    private float checkDot;
    [SerializeField]
    private LayerMask scenariMask;
    Vector2 rayDir;

    [Header("Chase Variables"), SerializeField]
    private float chaseSpeed;

    [Header("Attack Variables"), SerializeField]
    private float distanceToAttack;
    [SerializeField]
    private float orbitMoveSpeed;
    private float orbitAccumulator = 0;
    [SerializeField]
    private float orbitTargetRotationsSpeed;
    [SerializeField]
    private float orbitDistance;
    Vector2 posToOrbit = Vector2.zero;


    private NavMeshAgent agent;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        StatesFunctions();
    }

    private void StatesFunctions()
    {
        switch (currentState)
        {
            case EnemyState.WAITING:
                CheckIfPlayerNear();
                break;
            case EnemyState.PATROLING:
                CheckIfPlayerNear();
                break;
            case EnemyState.CHASING:
                //Seguir al jugador
                ChasePlayer();
                //Mirar a la direccion en la que va 
                LookForward();
                //Comprobar la distancia entre el y el player
                CheckDistanceBtwPlayer();
                break;
            case EnemyState.ATTACKING:
                //Mirar al player
                LookAtPlayer();
                //Dar vueltas alrededor del player
                OrbitPlayer();
                //Revisar tambien si esta muy lejos el player que le vuelva a perseguir
                CheckDistanceToChase();

                break;
            default:
                break;
        }

    }


    #region Detection Functions
    private void CheckIfPlayerNear() 
    {
        float distanceBtwPlayer = Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position);

        if (distanceBtwPlayer < maxVisionDistance) 
        {
            //El Player esta lo suficientemente cerca como para mirar donde esta
            CheckPlayerDotProduct();
        }

    }
    private void CheckPlayerDotProduct()
    {
        rayDir = EnemiesManager._instance.playerRef.transform.position - transform.position;

        float dot = Vector2.Dot(transform.up, rayDir.normalized);
        if (dot >= checkDot)
        {
            //Comprobar si hay algun muro delante
            CheckWallsBetween();
        }


    }
    private void CheckWallsBetween() 
    {

        rayDir = EnemiesManager._instance.playerRef.transform.position - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position), scenariMask);

        if (hit.collider == null)
        {
            SeePlayer();
        }
    }
    public void SeePlayer() 
    {
        StartChasing();

    }


    #endregion

    #region Patrol Functions

    #endregion

    #region Chase Functions
    private void ChasePlayer() 
    {
        agent.SetDestination(EnemiesManager._instance.playerRef.transform.position);
        Debug.Log("Aqui voy :D");
    }
    private void LookForward() 
    {
        float angle = Mathf.Atan2(agent.velocity.y, agent.velocity.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;

    }
    private void CheckDistanceBtwPlayer() 
    {
        if (Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position) <= distanceToAttack)
        {
            StartAttacking();
        }
    }
    private void StartChasing() 
    {
        currentState = EnemyState.CHASING;
        agent.speed = chaseSpeed;
    }

    #endregion

    #region AttackFunctions
    private void StartAttacking() 
    {
        currentState = EnemyState.ATTACKING;
        agent.speed = orbitMoveSpeed;
    }

    private void LookAtPlayer() 
    {
        Vector2 lookAtDir = EnemiesManager._instance.playerRef.transform.position - transform.position;

        float angle = Mathf.Atan2(lookAtDir.y, lookAtDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;

        Debug.Log("GIRASIOOOOON");
    }
    private void OrbitPlayer() 
    {
        orbitAccumulator += Time.fixedDeltaTime * orbitTargetRotationsSpeed;
        Vector2 playerPos = new Vector2(EnemiesManager._instance.playerRef.transform.position.x, EnemiesManager._instance.playerRef.transform.position.y);
        Vector2 orbitPos = new Vector2(Mathf.Cos(orbitAccumulator), Mathf.Sin(orbitAccumulator)) * orbitDistance;
        posToOrbit = playerPos + orbitPos;
        agent.SetDestination(posToOrbit);

        Debug.Log("GIRASIOOOOON");

    }
    private void CheckDistanceToChase() 
    {
        if (Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position) > distanceToAttack)
        {
            StartChasing();
        }
    }

    #endregion


    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxVisionDistance);

        if (EnemiesManager._instance)
        {
            rayDir = EnemiesManager._instance.playerRef.transform.position - transform.position;
            float dot = Vector2.Dot(transform.up, rayDir.normalized);

            if (dot >= checkDot)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }


            Gizmos.DrawRay(transform.position, rayDir);
        }
        

    }

}
