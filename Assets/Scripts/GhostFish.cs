using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GhostFish : FishGeneral {
    private int _a, _b;
    private float _bigA, _bigB;
    private float _delta;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Ready)
        {
            Move ();
        }
    }

    private void Move() {
        SwimAnimation ();
        if (transform.position == StartPosRelative)
        {
            _a = Random.Range (1, 10);
            _b = Random.Range (1, 10);
            _bigA = Random.Range (-10, 10);
            _bigB = Random.Range (-10, 10);
            _delta = Random.Range (-Mathf.PI, Mathf.PI);
        }
        Tc += Time.deltaTime*speed;
        float x = _bigA * Mathf.Sin (_a * Tc + _delta) * MapManager.Scale;
        float z = _bigB * Mathf.Cos (_b * Tc) * MapManager.Scale;
        transform.position =  StartPosRelative + new Vector3 (x, 0, z);
        transform.rotation = Quaternion.LookRotation (new Vector3 (x, 0, z))*Quaternion.Euler (0,90,0);
    }

    protected void Awake() {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    protected void OnDestroy() {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    void GameManagerOnGameStateChanged(GameState state) {
        base.GameManagerOnGameStateChanged (state);
        if (state == GameState.Game)
        {
            Ready = true;
        }
    }
}
