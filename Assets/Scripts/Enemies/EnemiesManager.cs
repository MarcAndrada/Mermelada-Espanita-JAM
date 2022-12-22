using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{

    public static EnemiesManager _instance;

    public GameObject playerRef;

    private void Awake()
    {

        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(_instance);
                _instance = this;
            }
        }
        else
        {
            _instance = this;
        }
    }
}
