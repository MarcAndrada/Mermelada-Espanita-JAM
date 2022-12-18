using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    public event Action OnHit = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hola como estamos");
        if (other)
        {
            Destroy(other.gameObject);
        }
        OnHit();
    }
}
