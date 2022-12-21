using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int speed = 30;
    public float accelSpeed;
    public float dragSpeed;
    private float acceleration = 0;
    private Vector2 inputs;
    private Rigidbody2D rb2d;
    

    public static Action<bool> OnMove = delegate { };

    // Start is called before the first frame update
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
    }

    private void FixedUpdate()
    {
        ProcessInputs();
    }


    private void CheckInputs() 
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        inputs = inputs.normalized;
    }

    private void ProcessInputs()
    {
        if (inputs != Vector2.zero)
        {
            acceleration += accelSpeed * Time.fixedDeltaTime;
            acceleration = Mathf.Clamp(acceleration, 0, 1);
            rb2d.velocity = inputs * speed * acceleration * Time.fixedDeltaTime;
            OnMove(true);
        }
        else
        {
            acceleration -= dragSpeed * Time.fixedDeltaTime;
            acceleration = Mathf.Clamp(acceleration, 0, 1);
            rb2d.velocity = rb2d.velocity.normalized * speed * acceleration * Time.fixedDeltaTime;
            OnMove(false);
        }

    }
<<<<<<< HEAD
=======

    private void ObjectMovement(float speed)
    {
        acceleration += accelSpeed * Time.fixedDeltaTime;
        acceleration = Mathf.Clamp(acceleration, 0, 1 );
        rb2d.velocity = inputs * speed * acceleration * Time.fixedDeltaTime;
    }

>>>>>>> main
}
