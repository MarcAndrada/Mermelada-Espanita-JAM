using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;


public class Frenesis : MonoBehaviour {
    [Header("ComboSystem")]
    [SerializeField] private float maxTimeCombo;
    [SerializeField] private float currentTimeCombo;
    [SerializeField] private int comboLevel;

    [Header("Interface")]
    [SerializeField] private Sprite[] combosLevelsTextures;
    [SerializeField] private Image image;
    [SerializeField] private Image comboBar;

    [Header("PostProcessing")]
    [SerializeField] private GameObject pVolumeGameObject;
    private PostProcessVolume volume;
    Vignette vignette;
    ChromaticAberration chromaticAberration;
    Grain grain;

    private void Start() {
        volume = pVolumeGameObject.AddComponent<PostProcessVolume>();
        volume.profile = new PostProcessProfile();
        volume.isGlobal = true;

        CreateVignette();
        CreateChromaticAberration();
        CreateGrain();
    }
    
    private void CreateVignette() {
        vignette = volume.profile.AddSettings<Vignette>();

        vignette.enabled.Override(true);
        vignette.intensity.Override(0.15f);
        vignette.color.Override(Color.red);
        vignette.smoothness.Override(0.36f);
        vignette.roundness.Override(1f);
    }
    private void CreateChromaticAberration() {
        chromaticAberration = volume.profile.AddSettings<ChromaticAberration>();

        chromaticAberration.enabled.Override(true);
        chromaticAberration.intensity.Override(0.5f);
    }
    private void CreateGrain() {
        grain = volume.profile.AddSettings<Grain>();

        grain.enabled.Override(true);
        grain.colored.Override(true);
        grain.intensity.Override(1f);
        grain.size.Override(3f);
        grain.lumContrib.Override(0.8f);
    }

    private void Update()
    {
        comboBar.fillAmount = currentTimeCombo / maxTimeCombo;
        image.sprite = combosLevelsTextures[comboLevel];

        // Cambiar por kills conseguidas
        if (Input.GetKeyDown(KeyCode.P) && comboLevel < 2 )
        {
            comboLevel++;
            currentTimeCombo = maxTimeCombo;
        }

        if (comboLevel >= 0) {
            vignette.intensity.Override(0.15f);
        }
        if (comboLevel >= 1) {
            vignette.intensity.Override(0.25f);
        }
        if (comboLevel >= 2) {
            vignette.intensity.Override(0.5f);
        }

        if (comboLevel >= 0 && comboLevel < 3)
        {
            currentTimeCombo -= Time.deltaTime;
            if (currentTimeCombo <= 0 && comboLevel != 0)
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
        //pVolumeGameObject.SetActive(true);
    }
    private void ExitFrenesisMode()
    {
        //pVolumeGameObject.SetActive(false);
    }
}
