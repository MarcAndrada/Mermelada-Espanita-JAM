using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlateMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Collider2D coll2d;

    private bool plateBreak = false;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();

    }

    private void FixedUpdate()
    {
        CheckIfPlateDestroyed();
    }

    private void CheckIfPlateDestroyed()
    {
        if (rb2d.velocity.magnitude < 0.3f && !plateBreak)
        {
            BreakPlate();

        }
    }

    void BreakPlate()
    {
        //Animacion de romper plato

        //Desactivar la colision
        coll2d.enabled = false;
        //Parar el movimiento
        rb2d.velocity = Vector2.zero;
        rb2d.simulated = false;

        plateBreak = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies"))
        {
            BreakPlate();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Scenari"))
        {
            BreakPlate();
        }
    }

}
