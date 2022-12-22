using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rb2d;
    private PlayerMovement movementController;
    private PlayerAnimController playerAnim;
    private MousePosition mousePosController;
    private InputSystem inputSystem;
    private TakeObject takeObject;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        movementController = GetComponent<PlayerMovement>();
        mousePosController = GetComponent<MousePosition>();
        inputSystem = GetComponent<InputSystem>();
        takeObject = GetComponent<TakeObject>();
        playerAnim = GetComponent<PlayerAnimController>();

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
        SoundManager._soundMInstance.Play("DeathSound");

    }

    public void ResetPlayerState() 
    {
        if (takeObject._plate != null)
        {
            takeObject._plate.transform.parent = null;
            takeObject._hasObject = false;

        }
        //Cambiarle la animacion
        animator.SetBool("Dead", false);
        //Activar los scripts del player
        movementController.enabled = true;
        mousePosController.enabled = true;
        inputSystem.enabled = true;
        playerAnim.DisableAttackCollider();
        playerAnim._isAttacking = false;

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectile"))
        {
            KillPlayer(collision.transform.position);
        }
    }



}
