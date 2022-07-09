using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bubble_controler : MonoBehaviour {
    public ParticleSystem ps;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S))
        {
            if (!ps.isEmitting)
            {
                ps.Play();
            }
            
        }
        else
        {
           ps.Stop();
        }
    }
}
