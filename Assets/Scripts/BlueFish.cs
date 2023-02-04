using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BlueFish : FishGeneral {
    [SerializeField] private int range = 10;
    [SerializeField] private PlayerMechanics player;
    private SphereCollider _collider;
    private int _stepId = 1;
    private List<Vector3> _steps;
    private bool _goal = false;
    
    
    //leads player to the exit. Limited steps. Starts when player enters the room
    // Start is called before the first frame update
    void Start() {}
    protected void Awake() {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
        _collider = GetComponent<SphereCollider>();
    }

    protected void OnDestroy() {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    new void GameManagerOnGameStateChanged(GameState state) {
        base.GameManagerOnGameStateChanged (state);
        if (state == GameState.Game)
        {
            _steps = new List<Vector3> ();
            var tmp = Map.Escape (new Vector3Int (StartPosFixed.x, StartPosFixed.z));
            foreach (var pos in tmp)
            {
                _steps.Add (new Vector3 (pos.x + 0.5f, 0.5f, pos.y + 0.5f) * MapManager.Scale);
            }

            Ready = true;
        }
    }
    
    // Update is called once per frame
    void OnTriggerEnter(Collider other) {
        
            if (other.gameObject.tag == "Player")
            {
                _collider.isTrigger = true;
                if (_stepId < range && _goal == false)
                {
                    _goal = true;
                    transform.rotation = Quaternion.LookRotation (_steps[_stepId]-transform.position)*Quaternion.Euler (0,180,0);
                }
            }

    }

    void Update()
    {
        if (Ready)
        {
            Move();
            if (_stepId == range)
            {
                _collider.isTrigger = true;
            }
        }
    }

    void Move() {
        if (_goal)
        {
            SwimAnimation ();
            transform.position = Vector3.MoveTowards (transform.position, _steps[_stepId], speed*Time.deltaTime);
            if (transform.position == _steps[_stepId])
            {
                _stepId++;
                _goal = false;
                _collider.isTrigger = false;
                StartPosRelative = transform.position;
                CalculateStartPosFixed ();
            }
        }
        else
        {
            Circle ();
        }
    }
}
