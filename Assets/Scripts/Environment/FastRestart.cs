using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastRestart : MonoBehaviour
{
    private EnemyBehaviourController[] enemies;
    private PlateMovement[] throwable;
    private PlayerController player;

    private Vector3[] enemiesPositions;
    private Quaternion[] enemiesRotations;
    private Vector3[] throwablePositions;
    private Quaternion[] throwableRotations;
    private Vector3 playerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GameObject[] throwableObj = GameObject.FindGameObjectsWithTag("Object");
        throwable = new PlateMovement[throwableObj.Length];
        for (int i = 0; i < throwable.Length; i++)
        {
            throwable[i] = throwableObj[i].GetComponent<PlateMovement>();
        }
        GameObject[] enemiesObj = GameObject.FindGameObjectsWithTag("Enemies");
        enemies = new EnemyBehaviourController[enemiesObj.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemiesObj[i].GetComponent<EnemyBehaviourController>();
        }


        enemiesPositions = new Vector3[enemies.Length];
        enemiesRotations = new Quaternion[enemies.Length];
        throwablePositions = new Vector3[throwable.Length];
        throwableRotations = new Quaternion[throwable.Length];
        SaveAllEnteties();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            LoadAllEnteties();
        }
    }

    private void SaveAllEnteties() {
        for (int i = 0; i < enemies.Length; i++) {
            enemiesPositions[i] = enemies[i].transform.position;
            enemiesRotations[i] = enemies[i].transform.rotation;
        }
        for (int i = 0; i < throwable.Length; i++)
        {
            throwablePositions[i] = throwable[i].transform.position;
            throwableRotations[i] = throwable[i].transform.rotation;
        }
        playerPosition = player.transform.position;
    }
    private void LoadAllEnteties() {
        player.transform.position = playerPosition;
        player.ResetPlayerState();

        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].transform.position = enemiesPositions[i];
            enemies[i].transform.rotation = enemiesRotations[i];
            enemies[i].ResetEnemyState(enemiesPositions[i]);
        }
        for (int i = 0; i < throwable.Length; i++) {
            throwable[i].transform.position = throwablePositions[i];
            throwable[i].transform.rotation = throwableRotations[i];
            throwable[i].ResetPlate();
            throwable[i].enabled = false;
            throwable[i].gameObject.tag = "Object";
        }
       
    }

}
