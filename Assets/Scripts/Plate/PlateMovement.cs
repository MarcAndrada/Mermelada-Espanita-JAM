using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlateMovement : MonoBehaviour
{
    [SerializeField] private int speed = 70;
    [SerializeField] private float dragSpeed = 5;
    private float acceleration = 1;

    public Vector2 Direction { private get; set; }

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        acceleration -= dragSpeed * Time.fixedDeltaTime;
        acceleration = Math.Clamp(acceleration, 0, 1);
        rb2d.velocity = new Vector2(0, 1) * speed * acceleration * Time.fixedDeltaTime;
    }

    void DeadPlate()
    {
         Destroy(gameObject);
    }
}
