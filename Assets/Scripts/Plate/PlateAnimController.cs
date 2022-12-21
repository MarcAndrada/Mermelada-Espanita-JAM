using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateAnimController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
}
