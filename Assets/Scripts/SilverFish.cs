using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.LegacyInputHelpers;

public class SilverFish : FishGeneral {
    [SerializeField] private int range = 5;
    [SerializeField] private float speed = 1f;
    private Vector3Int _start;
    private Vector3 _finish;
    private List<Vector3> _steps;
    private MapManager _map;
    private int _stepId = 0;
    private bool _ready = false;
    
    void Start() {
        
    }
    
    void Awake() {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    void OnDestroy() {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }
    
    void GameManagerOnGameStateChanged(GameState state) {
        if (state == GameState.Game)
        {
            _map = FindObjectOfType <MapManager> ();
            _start = new Vector3Int (Random.Range (0, _map.GetWidth ()), Random.Range (0, _map.GetHeight ()), 0);
            _steps = new List<Vector3> ();
            GenerateRoute ();
            _start = new Vector3Int (_start.x, 0, _start.y);
            _finish = new Vector3 (_steps[0].x, Random.Range (0.0f, 1.0f), _steps[0].y);
            transform.position = _start;
            _ready = true;
        }
    }

    void GenerateRoute() {
        var pos = _start;

        for (int i = 0; i < range; i ++)
        {
            var acc = _map.GetAccessible (pos);
            var next = new List<Vector3Int> ();
            for (int j = 0; j < 4; j ++)
            {
                if (!acc[j])
                {
                    next.Add (new Vector3Int (pos.x+_map._neighbours[j,0], pos.y+_map._neighbours[j,1], 0));
                }
            }
            pos = next[Random.Range (0, next.Count)];
            var tries = 0;
            while (_steps.Contains (pos))
            {
                if (tries == 4)
                {
                    break;
                }
                tries ++;
                pos = next[Random.Range (0, next.Count)];
            }
            _steps.Add (pos);
        }

        for (int i = 1; i < range; i ++)
        {
            _steps.Add (_steps[range-1-i]);
        }
    }

    // Update is called once per frame
    void Update() {
        if (_ready)
        {
            Swim ();
            transform.position = Vector3.MoveTowards (transform.position, _finish, speed*Time.deltaTime);
            if (transform.position == _finish)
            {
                _stepId = (_stepId + 1) % _steps.Count;
                _finish = new Vector3 (_steps[_stepId].x, Random.Range (0.0f, 1.0f), _steps[_stepId].y);
            }
        }
        
    }
}
