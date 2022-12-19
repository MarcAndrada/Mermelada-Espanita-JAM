using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInputSystem : MonoBehaviour
{
    public event Action OnAttack = delegate { };
    public event Action OnThrow = delegate { };

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnThrow();
        }
    }
}
