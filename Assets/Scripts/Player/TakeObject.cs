using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeObject : MonoBehaviour
{

    public bool hasObject;
    [SerializeField] private GameObject objectToParent;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Object") {
            hasObject = true;
            collision.gameObject.transform.parent = objectToParent.transform;
            collision.gameObject.transform.position = objectToParent.transform.position;
        }
    }
}
