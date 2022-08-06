using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private int move = 0;
    private int deg = 40;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 goal = transform.position;
        if(Input.GetKey(KeyCode.Space))
        {
            goal += transform.up;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            goal -= transform.up;
        }
        if(Input.GetKey(KeyCode.W))
        {
            goal -= transform.forward;
            move = 1;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            goal += transform.forward;
            move = -1;
        }
        else
        {
            move = 0;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0,-deg*Time.deltaTime*move,0);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0,deg*Time.deltaTime*move,0);
        }

        //transform.Rotate(transform.LookAt (goal));
        transform.position = Vector3.MoveTowards(transform.position, goal, 4f * Time.deltaTime);
    }
}