using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    private float posX;
    private float posY;
    private float posZ;

    public float expansionRange = 10;

    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mousePosition.x > 900 && transform.position.x < Player.transform.position.x + expansionRange)
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, posZ);
        }

        if (Input.mousePosition.x < 100 && transform.position.x > Player.transform.position.x - expansionRange)
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, posZ);
        }

        if (Input.mousePosition.y > 400 && transform.position.y < Player.transform.position.y + expansionRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 2, posZ);
        }

        if (Input.mousePosition.y < 100 && transform.position.y > Player.transform.position.y - expansionRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 2, posZ);
        }

        Debug.Log(Input.mousePosition);
    }
}
