using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenesis : MonoBehaviour {
    [SerializeField] private float timeToReset;
    [SerializeField] private int killsToUpgrade;
    [SerializeField] private int currentKills;
    [SerializeField] private int frenesisLevel;
    private void Update() {
        if (currentKills > killsToUpgrade) {
            frenesisLevel++;
        }
    }
}
