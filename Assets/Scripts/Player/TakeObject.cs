using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private bool _hasObject;
    [SerializeField]
    private float lunchForce;

    public bool HasObject => _hasObject;

    [SerializeField] private GameObject objectToParent;
    [SerializeField] private GameObject _plate;

    private void OnEnable()
    {
        InputSystem.OnThrow += ThrowPlate;
    }

    private void OnDisable()
    {
        InputSystem.OnThrow += ThrowPlate;
    }

    private void ThrowPlate()
    {
        if (!_hasObject) return;

        _plate.transform.SetParent(null);
        _plate.gameObject.tag = "PlayerProjectile";
        Rigidbody2D plateRb = _plate.GetComponent<Rigidbody2D>();
        plateRb.simulated = true;
        plateRb.AddForce(transform.up * lunchForce, ForceMode2D.Impulse);
        PlateMovement plateScript =  _plate.GetComponent<PlateMovement>();
        plateScript.parentType = PlateMovement.ParentType.PLAYER;
        plateScript.enabled = true;
        _plate = null;
        _hasObject = false;
        
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Object"))
        {
            _hasObject = true;
            _plate = col.gameObject;
            _plate.GetComponent<Rigidbody2D>().simulated = false;
            _plate.transform.SetParent(objectToParent.transform);
            _plate.transform.localPosition = Vector2.zero;
        }
    }
}