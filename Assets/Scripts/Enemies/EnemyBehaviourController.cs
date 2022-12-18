using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourController : MonoBehaviour
{
    public enum EnemyState { WAITING, PATROLING, CHASING, ATTACKING };
    public EnemyState currentState;

    private EnemyMovementController movementController;

    [Header("DetectionVariables"), SerializeField]
    private float checkRange;
    [SerializeField]
    private float checkDot;

    private void Awake()
    {
        movementController = GetComponent<EnemyMovementController>();

    }

    // Update is called once per frame
    void Update()
    {



    }


    #region Detection Functions
    private void CheckPlayerDotProduct()
    {
        Vector3 playerDir = Vector3.zero;

        float dot = Vector2.Dot(transform.forward, playerDir.normalized);

        if (true)
        {

        }

    }

    private void CheckIfSeePlayers() 
    {
        
    }

    #endregion

}
