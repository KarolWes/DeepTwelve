using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bubble_controler : MonoBehaviour {
    [SerializeField] private GameObject submarine;
    private Vector3 _pos;
    public ParticleSystem ps;
    void Start() {
        _pos = submarine.transform.position +  new Vector3(0f, 0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        _pos = submarine.transform.position +  new Vector3(0f, 0f, 3f);
        transform.position = _pos;
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
