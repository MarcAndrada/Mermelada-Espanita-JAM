using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObject : MonoBehaviour
{
    [SerializeField] private bool _hasObject;
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
        _plate.GetComponent<PlateMovement>().enabled = true;
    }
    

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Object")) {
            _hasObject = true;
            _plate = col.gameObject;
            col.gameObject.transform.parent = objectToParent.transform;
            col.gameObject.transform.position = objectToParent.transform.position;
        }
    }
}
