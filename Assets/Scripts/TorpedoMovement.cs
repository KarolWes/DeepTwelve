using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TorpedoMovement : MonoBehaviour {
    private TorpedoManager _tm;
    [SerializeField] private float maxDist = 5f;
    [SerializeField] private float speed = 5f;
    void Start() {
        Vector3 pos = transform.position;
        _tm = FindObjectOfType<TorpedoManager> ();
        transform.localScale = new Vector3 (0.4f, 0.6f, 0.4f);
        Vector3 goal = pos + new Vector3 (0, 0, -maxDist);
        transform.position = Vector3.MoveTowards (pos, goal, speed*Time.deltaTime); // nie działa
        Destroy (this);
    }

}
