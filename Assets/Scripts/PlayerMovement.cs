using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
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
            goal += transform.forward;
        }
        if(Input.GetKey(KeyCode.S))
        {
            goal -= transform.forward;
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0,100*Time.deltaTime,0);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0,-100*Time.deltaTime,0);
        }
        transform.position = Vector3.MoveTowards(transform.position, goal, 4f * Time.deltaTime);
    }
}