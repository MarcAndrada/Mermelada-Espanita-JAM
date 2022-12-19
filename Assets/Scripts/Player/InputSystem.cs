using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public static Action OnAttack = delegate { };
    public static Action OnThrow = delegate { };

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
