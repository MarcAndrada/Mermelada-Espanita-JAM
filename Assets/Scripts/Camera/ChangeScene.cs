using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public TakeMissionObject missionObject;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && missionObject.playerHasObject == true) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
