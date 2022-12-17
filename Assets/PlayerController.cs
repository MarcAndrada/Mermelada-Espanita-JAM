using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        
        input = input.normalized;
        transform.position = new Vector2(transform.position.x, transform.position.y) + input * speed * Time.deltaTime;
        Debug.Log(input.magnitude);
    }
}
