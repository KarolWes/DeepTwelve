using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanics : MonoBehaviour
{
    private Vector3Int _posFixed;
    private Vector3 _posRelative;
    private MapManager _map;
    private int _points = 0;
    private int _shoal = 0;
    [SerializeField] private int shoalLimit = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log (_posFixed);
    }
    protected void Awake() {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    protected void OnDestroy() {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }
    protected void GameManagerOnGameStateChanged(GameState state) {
        if (state == GameState.Game)
        {
            _map = FindObjectOfType <MapManager> ();
            _posFixed = _map.GetStartPoint ();
            _posFixed.z = _posFixed.y;
            _posFixed.y = 0;
            CalculateStartPosRelative ();
        }
    }
    protected void CalculateStartPosRelative() {
        _posRelative = (_posFixed + new Vector3 (0.5f, 0.5f, 0.5f)) * MapManager.Scale;
    }
    
    void CalculatePosFix() {
        if (Math.Abs (transform.position.x - _posRelative.x) > MapManager.Scale / 2.0f || Math.Abs (transform.position.z - _posRelative.z) > MapManager.Scale / 2.0f)
        {
             _posFixed = Vector3Int.FloorToInt (transform.position / MapManager.Scale - new Vector3 (0.5f, 0.5f, 0.5f));
             CalculateStartPosRelative ();
        }
    }

    public Vector3Int GetPosFixed() {
        CalculatePosFix ();
        return _posFixed;
    }

    public Vector3 GetPosRelative() {
        return _posRelative;
    }

    public int GetShoalCount() {
        return _shoal;
    }

    public bool ChangeShoalCount() {
        _shoal ++;
        if (_shoal >= shoalLimit)
        {
            _shoal = 0;
            // penalty (turn of the light, random turn, etc)
            return true;
        }
        return false;
    }

    public void ChangePoints(int v) {
        _points += v;
        Debug.Log ("You now have " + _points + " points");
    }

    public int GetPoints() {
        return _points;
    }
}
