using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing;

    public Vector2 maxPos;
    public Vector2 minPos;

    private float posX;
    private float posY;
    private float posZ;

    public float expansionRange = 2;

    public Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        posZ = transform.position.z;
    }

    private void FixedUpdate()
    {
        if(Input.mousePosition.x < 900 || Input.mousePosition.x > 100 || Input.mousePosition.y < 400 || Input.mousePosition.y > 100)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

                targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
                targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);

                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }

        if (Input.mousePosition.x > 900 && transform.position.x < Player.transform.position.x + expansionRange)
        {
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, posZ);
        }

        if (Input.mousePosition.x < 100 && transform.position.x > Player.transform.position.x - expansionRange)
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, posZ);
        }

        if (Input.mousePosition.y > 800 && transform.position.y < Player.transform.position.y + expansionRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, posZ);
        }

        if (Input.mousePosition.y < 100 && transform.position.y > Player.transform.position.y - expansionRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, posZ);
        }

        //Debug.Log(Input.mousePosition);
    }
}
