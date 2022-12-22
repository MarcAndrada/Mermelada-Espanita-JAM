using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourController : MonoBehaviour
{
    public enum EnemyState { WAITING, PATROLING, CHASING, ATTACKING, STUNNED, DEAD };
    public enum PatrolType { RESTART, B_T_F };
    public EnemyState currentState;
    private EnemyState starterState;


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
    private Vector2 posToOrbit = Vector2.zero;
    [SerializeField]
    private float timeToWaitAttack;
    private float timeWaitedAttack;
    [SerializeField]
    private GameObject plate;
    [SerializeField]
    private float lunchForce;

    [Header("Detection Scream Variables"), SerializeField]
    private float screamArea;


    [Header("Stun Variables"), SerializeField]
    private float stunForce;
    [SerializeField]
    private float timeToWaitStunned;
    private float timeWaitedStunned = 0;

    private NavMeshAgent agent;
    private Rigidbody2D rb2d;
    private Animator animator;
    private Collider2D coll;

    AudioSource deathSound;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        starterState = currentState;
        timeWaitedAttack = timeToWaitAttack - 0.25f;
        deathSound = GetComponent<AudioSource>();
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
                LookAt(agent.velocity);
                break;
            case EnemyState.CHASING:
                //Seguir al jugador
                ChasePlayer();
                //Mirar a la direccion en la que va 
                LookAt(agent.velocity);
                //Comprobar la distancia entre el y el player
                CheckDistanceBtwPlayer();
                //Lanzarle cosas al player
                WaitToThrowPlate();
                break;
            case EnemyState.ATTACKING:
                //Mirar al player
                LookAt(EnemiesManager._instance.playerRef.transform.position - transform.position);
                //Dar vueltas alrededor del player
                OrbitPlayer();
                //Revisar tambien si esta muy lejos el player que le vuelva a perseguir
                CheckDistanceToChase();
                //Lanzarle cosas al player
                WaitToThrowPlate();
                break;
            case EnemyState.STUNNED:
                //Esperar para dejar de estar stuneado
                WaitToUnstunned();
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
        else
        {
            float distanceBtwPlayer = Vector2.Distance(transform.position, EnemiesManager._instance.playerRef.transform.position);
            if (distanceBtwPlayer <= 2)
            {
                CheckWallsBetween();
            }
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
        animator.SetBool("Walking", true);
    }

    #endregion

    #region Attack Functions
    private void StartAttacking()
    {
        currentState = EnemyState.ATTACKING;
        agent.speed = orbitMoveSpeed;
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
    private void WaitToThrowPlate() 
    {
        timeWaitedAttack += Time.fixedDeltaTime;
        if (timeWaitedAttack >= timeToWaitAttack)
        {
            ThrowPlate();
            timeWaitedAttack = 0;
        }

    }

    private void ThrowPlate() 
    {
        GameObject thisPlate = Instantiate(plate,transform.position,transform.rotation);
        thisPlate.gameObject.tag = "EnemyProjectile";
        
        Rigidbody2D plateRb = thisPlate.GetComponent<Rigidbody2D>();
        plateRb.simulated = true;
        plateRb.AddForce(transform.up * lunchForce, ForceMode2D.Impulse);
        
        PlateMovement plateScript =  thisPlate.GetComponent<PlateMovement>();
        plateScript.parentType = PlateMovement.ParentType.ENEMY;
        plateScript.enabled = true;

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

    #region Stun Functions

    private void WaitToUnstunned() 
    {
        timeWaitedStunned += Time.fixedDeltaTime;

        if (timeWaitedStunned >= timeToWaitStunned) 
        {
            agent.enabled = true;
            //Cambiarle el estado
            StartChasing();
            //Hacer que la animacion este levantado
            animator.SetBool("Stunned", false);
            //Resetear el timer
            timeWaitedStunned = 0;


        }
    }

    #endregion

    #region Damage Functions

    private void HittedByObject(Vector2 _objectPos) 
    {
        //Comprobar si el medidor de combo es menor a X stunearle si no matarle
<<<<<<< Updated upstream
        if (true)
=======
        if (Frenesis._instance.comboLevel < 3)
>>>>>>> Stashed changes
        {
            StunEnemy(_objectPos);
        }
        else 
        {
            KillEnemy(_objectPos);
        }
    
    }

    private void KillEnemy(Vector2 _killerPos) 
    {
        //Poner el estado de muerto
        currentState = EnemyState.DEAD;
        //Empujar al enemigo
        Vector2 knockBackDir = new Vector2(transform.position.x, transform.position.y) - _killerPos;
        agent.enabled = false;
        rb2d.AddForce(knockBackDir * stunForce, ForceMode2D.Impulse);

        //Mirar a la direccion del golpe
        LookAt(_killerPos - (knockBackDir * 2));
        //Animacion de muerto
        animator.SetBool("Dead", true);
        animator.SetTrigger("Death");
        //Desactivarle la colision
        coll.enabled = false;

        deathSound.Play();
<<<<<<< Updated upstream
=======
        Frenesis._instance.IncrementFrenesi();
>>>>>>> Stashed changes

    }

    private void StunEnemy(Vector2 _stunnerPos) 
    {
        //Cambiar el estado
        currentState = EnemyState.STUNNED;
        //Empujar
        Vector2 knockBackDir = new Vector2(transform.position.x, transform.position.y) - _stunnerPos;

        agent.enabled = false;

        rb2d.AddForce(knockBackDir * stunForce, ForceMode2D.Impulse);
        
        //Mirar en la direccion del golpe
        LookAt(_stunnerPos - (knockBackDir * 2));
        //Animacion de tumbado
        animator.SetBool("Stunned", true);
        animator.SetTrigger("Stun");
    }

    #endregion

    private void LookAt(Vector2 _lookAtDir)
    {
        float angle = Mathf.Atan2(_lookAtDir.y, _lookAtDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;
    }

    public void ResetEnemyState(Vector2 _starterPos) 
    {
        agent.enabled = true;
        coll.enabled = true;
        currentState = starterState;
        agent.SetDestination(_starterPos);
        timeWaitedAttack = timeToWaitAttack - 0.25f;
        if (currentState == EnemyState.WAITING)
        {
            animator.SetBool("Walking", false);
        }
        animator.SetBool("Dead", false);
        animator.SetBool("Stunned", false);

       

    }


<<<<<<< Updated upstream
    private void OnDrawGizmos()
=======
    private void OnDrawGizmosSelected()
>>>>>>> Stashed changes
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
        }

        if (collision.gameObject.CompareTag("PlayerDamageColl"))
        {
            KillEnemy(collision.transform.position);
        }
    }

}
