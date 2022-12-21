using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    // [SerializeField] private float _attackResetTime = 0.12f;
    [SerializeField] private bool _isAttacking;
    private Animator _animator;
    [SerializeField]
    private Collider2D coll2D;

    AudioSource hitSound;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        hitSound = GetComponent<AudioSource>();
    }

    #region EventSubscribers

    private void OnEnable()
    {
        PlayerMovement.OnMove += WalkAnim;
        InputSystem.OnAttack += AttackAnim;
        InputSystem.OnThrow += ThrowAnim;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMove -= WalkAnim;
        InputSystem.OnAttack -= AttackAnim;
        InputSystem.OnThrow -= ThrowAnim;
    }

    #endregion

    private void WalkAnim(bool state)
    {
        _animator.SetBool("Walking", state);
    }
    
    private void AttackAnim()
    {
        if (_isAttacking) return;
        _isAttacking = true;
        _animator.SetTrigger("Attack");
        hitSound.Play();
    }
    
    private void ThrowAnim()
    {
        //Debug.Log("Hola como estamos");
    }

    private void ResetAttack()
    {
        _isAttacking = false;
    }

    public void EnableAttackCollider() 
    {
        coll2D.enabled = true;
    }

    public void DisableAttackCollider() 
    {
        coll2D.enabled = false;
    }


 

}
