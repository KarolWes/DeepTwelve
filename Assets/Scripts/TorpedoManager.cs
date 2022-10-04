using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorpedoManager : MonoBehaviour
{

    [SerializeField] private GameObject _torpedo;
    private int slot = 1;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Spawn();
            slot *= -1;
        }
    }

    void Spawn()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        rot = rot * Quaternion.Euler(-90f, 0, 0);
        pos += new Vector3(slot * 0.26f, -0.13f, 0f);
        Instantiate(_torpedo, pos, rot);

    }
}
