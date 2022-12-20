using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{

    [SerializeField] private float amp;

    private Vector3 basePosition;
    // Update is called once per frame
    private void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(basePosition.x, Mathf.Sin(Time.time) * amp + basePosition.y, 0);
    }
}
