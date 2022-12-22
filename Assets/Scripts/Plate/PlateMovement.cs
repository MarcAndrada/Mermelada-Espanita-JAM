using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlateMovement : MonoBehaviour
{
    public enum ParentType {PLAYER, ENEMY };
    public ParentType parentType;

    private Rigidbody2D rb2d;
    private Collider2D coll2d;
    private Animator animator;
    private bool plateBreak = false;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        coll2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetTrigger("Throw");
    }

    private void FixedUpdate()
    {
        CheckIfPlateDestroyed();
    }

    private void CheckIfPlateDestroyed()
    {
        if (rb2d.velocity.magnitude < 0.4f && !plateBreak)
        {
            BreakPlate();
            animator.SetTrigger("Fall");
        }
    }

    private void BreakPlate()
    {
        //Desactivar la colision
        coll2d.enabled = false;
        //Parar el movimiento
        rb2d.velocity = Vector2.zero;
        rb2d.simulated = false;

        plateBreak = true;
    }

    public void ResetPlate() 
    {
        coll2d.enabled = true;
        rb2d.simulated = true;
        plateBreak = false;
        animator.SetTrigger("Restart");     

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies") && parentType == ParentType.PLAYER || 
            collision.gameObject.CompareTag("Player") && parentType == ParentType.ENEMY ||
            collision.gameObject.layer == LayerMask.NameToLayer("Scenari"))
        {
            BreakPlate();
            animator.SetTrigger("Hit");
        }
    }

}
