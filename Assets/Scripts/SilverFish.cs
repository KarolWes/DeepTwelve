using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.LegacyInputHelpers;

public class SilverFish : FishGeneral {
    //swims a random path between two points
    [SerializeField] private int range = 5;
    private Vector3 _finish;
    private List<Vector3> _steps;
    private int _stepId = 0;
    private Renderer _fishRend;
    
    void Start() {
        _fishRend = GetComponent<Renderer> ();
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
            _steps = new List<Vector3> ();
            StartPosFixed = new Vector3Int (StartPosFixed.x, StartPosFixed.z, 0);
            GenerateRoute ();
            
            _finish = new Vector3 (_steps[0].x, Random.Range (0.0f, 1.0f)*MapManager.Scale, _steps[0].y);
            transform.position = new Vector3((StartPosFixed.x+0.5f)*MapManager.Scale, 0.5f*MapManager.Scale, (StartPosFixed.y+0.5f)*MapManager.Scale);
            transform.rotation = Quaternion.LookRotation (_finish); 
            Ready = true;
        }
    }

    void GenerateRoute() {
        var pos = StartPosFixed;

        for (int i = 0; i < range; i ++)
        {
            var acc = Map.GetAccessible (pos); //check
            var next = new List<Vector3Int> ();
            for (int j = 0; j < 4; j ++)
            {
                if (!acc[j])
                {
                    next.Add (new Vector3Int (pos.x+Map.Neighbours[j,0], pos.y+Map.Neighbours[j,1], 0));
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
        for(int i = 0; i < 2*range-1; i++)
        {
            _steps[i] = (_steps[i] + new Vector3 (0.5f, 0.5f, 0.5f)) * MapManager.Scale;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Ready)
        {
            SwimAnimation ();
            transform.position = Vector3.MoveTowards (transform.position, _finish, speed*Time.deltaTime);
            if (transform.position == _finish)
            {
                _stepId = (_stepId + 1) % _steps.Count;
                _finish = new Vector3 (_steps[_stepId].x, Random.Range (0.0f, 1.0f)*MapManager.Scale, _steps[_stepId].y);
                transform.rotation = Quaternion.LookRotation (_finish-transform.position);
            }
        }
        
    }
}
