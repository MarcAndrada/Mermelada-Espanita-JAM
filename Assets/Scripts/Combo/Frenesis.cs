using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;


public class Frenesis : MonoBehaviour
{
    [Header("ComboSystem")]
    [SerializeField] private float maxTimeCombo;
    [SerializeField] private float currentTimeCombo;
    [SerializeField] private int comboLevel;
    [SerializeField] private bool frenesis;

    [Header("Interface")]
    [SerializeField] private Sprite[] combosLevelsTextures;
    [SerializeField] private Image image;

    [Header("PostProcessing")]
    [SerializeField] private GameObject pVolume;

    private void Update()
    {
        image.sprite = combosLevelsTextures[comboLevel];

        if (Input.GetKeyDown(KeyCode.P) && comboLevel <= 2)
        {
            comboLevel++;
            currentTimeCombo = maxTimeCombo;
        }
        if (comboLevel > 0)
        {
            currentTimeCombo -= Time.deltaTime;
            if (currentTimeCombo <= 0)
            {
                currentTimeCombo = maxTimeCombo;
                comboLevel--;
            }
        }
        if (comboLevel >= 2)
        {
            EnterFrenesisMode();
        }
        if (comboLevel < 2)
        {
            ExitFrenesisMode();
        }
    }

    private void EnterFrenesisMode()
    {
        pVolume.SetActive(true);
    }
    private void ExitFrenesisMode()
    {
        pVolume.SetActive(false);
    }
}
