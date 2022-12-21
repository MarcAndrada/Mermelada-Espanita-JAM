using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb2d;
    private PlayerMovement movementController;
    private MousePosition mousePosController;
    private InputSystem inputSystem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        movementController = GetComponent<PlayerMovement>();
        mousePosController = GetComponent<MousePosition>();
        inputSystem = GetComponent<InputSystem>();
    }


    private void KillPlayer(Vector2 _damageDealerPos)
    {


        //Cambiarle la animacion
        animator.SetBool("Dead", true);
        animator.SetTrigger("Death");
        //Desactivar los scripts del player
        movementController.enabled = false;
        mousePosController.enabled = false;
        inputSystem.enabled = false;
        //Empujar el player 
        Vector2 knockBackDir = new Vector2(transform.position.x, transform.position.y) - _damageDealerPos;
        rb2d.AddForce(knockBackDir * 20, ForceMode2D.Impulse);

    }

    public void ResetPlayerState() 
    {
        //Cambiarle la animacion
        animator.SetBool("Dead", false);
        //Activar los scripts del player
        movementController.enabled = true;
        mousePosController.enabled = true;
        inputSystem.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyDamageColl"))
        {
            KillPlayer(collision.transform.position);
        }
    }



}
