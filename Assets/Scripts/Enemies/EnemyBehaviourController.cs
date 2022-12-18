using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviourController : MonoBehaviour
{
    public enum EnemyState { WAITING, PATROLING, CHASING, ATTACKING, DEAD };
    public EnemyState currentState;

    [Header("Detection Variables"), SerializeField]
    private float checkRange;
    [SerializeField]
    private float checkDot;
    [SerializeField]
    private LayerMask scenariMask;

    [Header("Attack Variables"), SerializeField]
    
    
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
                CheckPlayerDotProduct();
                break;
            case EnemyState.PATROLING:
                CheckPlayerDotProduct();
                break;
            case EnemyState.CHASING:
                //Seguir al jugador
                ChasePlayer();
                //Mirar a la direccion en la que va 
                LookForward();
                //Comprobar la distancia entre el y el player
                break;
            case EnemyState.ATTACKING:
                //Mirar al player
                LookAtPlayer();
                //Dar vueltas alrededor del player



                break;
            default:
                break;
        }

    }


    #region Detection Functions
    private void CheckPlayerDotProduct()
    {

    }

    private void SeePlayer() 
    {

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

        Debug.Log("GIRASIOOOOON");
    }

    #endregion


    #region AttackFunctions

    private void LookAtPlayer() 
    {


        Vector2 lookAtDir = EnemiesManager._instance.playerRef.transform.position - transform.position;

        float angle = Mathf.Atan2(lookAtDir.y, lookAtDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        transform.rotation = rotation;

        Debug.Log("GIRASIOOOOON");
    }

    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,1);

    }

}
