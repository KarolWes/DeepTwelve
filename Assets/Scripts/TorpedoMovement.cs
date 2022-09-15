using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TorpedoMovement : MonoBehaviour {
    //private TorpedoManager _tm;
    [SerializeField] private float maxDist = 5f;
    [SerializeField] private float speed = 5f;
    private Vector3 goal;
    void Start() {
        //_tm = FindObjectOfType<TorpedoManager> ();
        transform.localScale = new Vector3 (0.4f, 0.6f, 0.4f);
        goal = transform.position + new Vector3 (0, 0, maxDist);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards (transform.position, goal, speed*Time.deltaTime);
        if (transform.position == goal)
        {
            Destroy (this); // nie działa
        } 
    }

}
