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
            GenerateRoute ();
            StartPos = new Vector3Int (StartPos.x, 0, StartPos.y);
            _finish = new Vector3 (_steps[0].x*scale, Random.Range (0.0f, 1.0f)*scale, _steps[0].y*scale);
            transform.position = new Vector3(StartPos.x*scale, 0, StartPos.z*scale);
            transform.rotation = Quaternion.LookRotation (_finish); 
            Ready = true;
        }
    }

    void GenerateRoute() {
        var pos = StartPos;

        for (int i = 0; i < range; i ++)
        {
            var acc = Map.GetAccessible (pos); //check
            var next = new List<Vector3Int> ();
            for (int j = 0; j < 4; j ++)
            {
                if (!acc[j])
                {
                    next.Add (new Vector3Int (pos.x+Map._neighbours[j,0], pos.y+Map._neighbours[j,1], 0));
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
        if (Ready)
        {
            Swim ();
            transform.position = Vector3.MoveTowards (transform.position, _finish, speed*Time.deltaTime);
            if (transform.position == _finish)
            {
                _stepId = (_stepId + 1) % _steps.Count;
                _finish = new Vector3 (_steps[_stepId].x*scale, Random.Range (0.0f, 1.0f)*scale, _steps[_stepId].y*scale);
                transform.rotation = Quaternion.LookRotation (_finish-transform.position);
            }
        }
        
    }
}
