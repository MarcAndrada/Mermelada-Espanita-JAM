using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public GameObject doorOpened;
    public GameObject doorClosed;

    public Collider2D m_Collider;

    void Start()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            doorClosed.SetActive(false);
            doorOpened.SetActive(true);
            m_Collider.isTrigger = false;
        }
    }
}
