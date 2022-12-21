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
        //No funciona
        if (rb2d.velocity.magnitude < 0.5f)
        {
            DeadPlate();
        }
        acceleration -= dragSpeed * Time.fixedDeltaTime;
        acceleration = Math.Clamp(acceleration, 0, 1);
        rb2d.velocity = Direction * (speed * acceleration * Time.fixedDeltaTime);
    }

    void DeadPlate()
    {
        //Hay que mirar esto
         Destroy(gameObject);
    }
}
