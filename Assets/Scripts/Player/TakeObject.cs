using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private bool _hasObject;
    [SerializeField]
    private float lunchForce;

    public bool HasObject => _hasObject;

    [SerializeField] private GameObject objectToParent;
    [SerializeField] private GameObject _plate;
    private TakeObject _tk;

    private void Awake()
    {
        _tk = GetComponent<TakeObject>();
    }

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
        if (!_tk.HasObject) return;

        _plate.transform.SetParent(null);
        _plate.gameObject.tag = "PlayerProjectile";
        Rigidbody2D plateRb = _plate.GetComponent<Rigidbody2D>();
        plateRb.simulated = true;
        plateRb.AddForce(transform.up * lunchForce, ForceMode2D.Impulse);
        _plate.GetComponent<PlateMovement>().enabled = true;
        
    }
    

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Object")) {
            _hasObject = true;
            _plate = col.gameObject;
            _plate.GetComponent<Rigidbody2D>().simulated = false;
            _plate.transform.SetParent(objectToParent.transform);
            _plate.transform.localPosition = Vector2.zero;
        }
    }
}
