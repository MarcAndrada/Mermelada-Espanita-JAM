using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastRestart : MonoBehaviour
{
    private GameObject[] enemies;
    private GameObject[] throwable;
    private GameObject player;

    private Vector3[] enemiesPositions;
    private Vector3[] throwablePositions;
    private Vector3 playerPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        throwable = GameObject.FindGameObjectsWithTag("Object");
        enemies = GameObject.FindGameObjectsWithTag("Enemies");

        enemiesPositions = new Vector3[enemies.Length];
        throwablePositions = new Vector3[throwable.Length];

        for (int i = 0; i < enemies.Length; i++) {
            enemiesPositions[i] = enemies[i].transform.position;
        }
        for (int i = 0; i < throwable.Length; i++) {
            throwablePositions[i] = throwable[i].transform.position;
        }
        playerPosition = player.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            for (int i = 0; i < enemies.Length; i++) {
                enemies[i].transform.position = enemiesPositions[i];
                //enemies[i].resetState;
            }
            for (int i = 0; i < throwable.Length; i++) {
                throwable[i].transform.position = throwablePositions[i];
                //throwable[i].resetState;
            }
            player.transform.position = playerPosition;
        }
    }
}
