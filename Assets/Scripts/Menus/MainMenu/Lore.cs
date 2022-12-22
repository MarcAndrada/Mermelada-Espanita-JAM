using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lore : MonoBehaviour
{
    [SerializeField] private GameObject[] textToShow;

    public void ShowText() {
        foreach(GameObject text in textToShow) {
            text.SetActive(true);
        }
    }
    public void HideText() {
        foreach (GameObject text in textToShow) {
            text.SetActive(false);
        }
    }
}
