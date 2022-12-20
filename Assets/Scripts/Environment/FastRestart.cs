using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastRestart : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject[] throwable;
    private GameObject player;

    private Vector3[] enemiesPositions;
    private Quaternion[] enemiesRotations;
    private Vector3[] throwablePositions;
    private Quaternion[] throwableRotations;
    private Vector3 playerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwable = GameObject.FindGameObjectsWithTag("Object");
        enemies = GameObject.FindGameObjectsWithTag("Enemies");

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
        for (int i = 0; i < enemies.Length; i++) {
            enemies[i].transform.position = enemiesPositions[i];
            enemies[i].transform.rotation = enemiesRotations[i];
            //enemies[i].resetState;
        }
        for (int i = 0; i < throwable.Length; i++) {
            throwable[i].transform.position = throwablePositions[i];
            throwable[i].transform.rotation = throwableRotations[i];
            //throwable[i].resetState;
        }
        player.transform.position = playerPosition;
    }

}
