using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourController : MonoBehaviour
{
    public enum EnemyState { WAITING, PATROLING, CHASING, ATTACKING, STUNNED, DEAD };
    public enum PatrolType { RESTART, B_T_F };
    public EnemyState currentState;

    [Header("Detection Variables"), SerializeField]
    private float maxVisionDistance;
    [SerializeField]
    private float checkDot;
    [SerializeField]
    private LayerMask scenariMask;
    Vector2 rayDir;

    [Header("Patrol Variables"), SerializeField]
    public PatrolType patrolType;
    [SerializeField]
    private Transform[] patrolPoints;
    private int patrolIndex = -1;
    private bool ascendingPatrol = true;

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


    [Header("Detection Scream Variables"), SerializeField]
    private float screamArea;


    [Header("Stun Variables"), SerializeField]
    private float stunForce;



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
                //Comprobar si tiene el player en frente
                CheckIfPlayerNear();
                break;
            case EnemyState.PATROLING:
                //Comprobar si tiene el player en frente
                CheckIfPlayerNear();
                //Comprobar si tiene un camino que seguir
                CheckPath();
                //Mirar a la direccion en la que esta andando
                LookForward();
                break;
            case EnemyState.CHASING:
                //Seguir al jugador
                ChasePlayer();
                //Mirar a la direccion en la que va 
                LookForward();
                //Comprobar la distancia entre el y el player
                CheckDistanceBtwPlayer();
                //Lanzarle cosas al player

                break;
            case EnemyState.ATTACKING:
                //Mirar al player
                LookAtPlayer();
                //Dar vueltas alrededor del player
                OrbitPlayer();
                //Revisar tambien si esta muy lejos el player que le vuelva a perseguir
                CheckDistanceToChase();
                //Lanzarle cosas al player

                break;
            case EnemyState.STUNNED:
                //Esperar para dejar de estar stuneado
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
        DoScream();

    }


    #endregion

    #region Patrol Functions
    private void CheckPath()
    {
        if (!agent.hasPath)
        {
            agent.SetDestination(patrolPoints[GetNextPos()].position);
        }
    }
    private int GetNextPos()
    {
        switch (patrolType)
        {
            case PatrolType.RESTART:
                patrolIndex++;
                if (patrolIndex >= patrolPoints.Length)
                {
                    patrolIndex = 0;
                }
                break;
            case PatrolType.B_T_F:
                if (ascendingPatrol)
                {
                    patrolIndex++;
                    if (patrolIndex >= patrolPoints.Length)
                    {
                        patrolIndex = patrolPoints.Length - 1;
                        ascendingPatrol = false;
                    }

                }
                else
                {
                    patrolIndex--;
                    if (patrolIndex <= 0)
                    {
                        patrolIndex = 0;
                        ascendingPatrol = true;
                    }
                }
                break;
            default:
                break;
        }
        return patrolIndex;
    }


    #endregion

    #region Chase Functions
    private void ChasePlayer()
    {
        agent.SetDestination(EnemiesManager._instance.playerRef.transform.position);
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

    #region Attack Functions
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

    }
    private void OrbitPlayer()
    {
        orbitAccumulator += Time.fixedDeltaTime * orbitTargetRotationsSpeed;
        Vector2 playerPos = new Vector2(EnemiesManager._instance.playerRef.transform.position.x, EnemiesManager._instance.playerRef.transform.position.y);
        Vector2 orbitPos = new Vector2(Mathf.Cos(orbitAccumulator), Mathf.Sin(orbitAccumulator)) * orbitDistance;
        posToOrbit = playerPos + orbitPos;
        agent.SetDestination(posToOrbit);


    }
    private void CheckDistanceToChase()
    {
        if (Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position) > distanceToAttack)
        {
            StartChasing();
        }
    }

    #endregion

    #region Scream Functions

    private void DoScream()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, screamArea, Vector2.zero);

        foreach (RaycastHit2D item in hits)
        {
            if (item.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                item.transform.GetComponent<EnemyBehaviourController>().HearedScream();
            }
        }


    }

    public void HearedScream()
    {
        StartChasing();
    }

    #endregion

    #region Damage Functions

    private void HittedByObject(Vector3 _objectPos) 
    {
        //Comprobar si el medidor de combo es menor a X stunearle si no matarle
        if (true)
        {
            StunEnemy(_objectPos);
        }
        else 
        {
            KillEnemy(_objectPos);
        }
    
    }

    private void KillEnemy(Vector3 _killerPos) 
    {
        //Poner el estado de muerto

        //Mirar a la direccion del golpe

        //Animacion de muerto
    }

    private void StunEnemy(Vector3 _stunnerPos) 
    {
        //Cambiar el estado
        currentState = EnemyState.STUNNED;
        //Empujar
        Vector2 knockBackDir = transform.position - _stunnerPos;

        if (Physics2D.Raycast(transform.position, knockBackDir, stunForce, LayerMask.NameToLayer("Scenari")))
        {
            ContactFilter2D contactfilters = new ContactFilter2D();
            RaycastHit2D[] hit = new RaycastHit2D[1];
            Physics2D.Raycast(transform.position, knockBackDir, contactfilters, hit, stunForce);

            knockBackDir = hit[0].point + (-knockBackDir * 1.5f);
            
        }
        else
        {
            knockBackDir = new Vector2(transform.position.x, transform.position.y) + (knockBackDir * stunForce);
        }

        agent.SetDestination(knockBackDir);
        //Mirar en la direccion del golpe

        //Animacion de tumbado


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


        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, screamArea);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            HittedByObject(collision.transform.position);
            Debug.Log("Trigger");
        }

        //if (collision.gameObject.CompareTag("PlayerDamageColl"))
        //{
        //    KillEnemy();
        //}
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            HittedByObject(collision.transform.position);
            Destroy(collision.gameObject);
            Debug.Log("Colision");
        }
    }

}
