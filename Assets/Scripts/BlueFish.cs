using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BlueFish : FishGeneral {
    private List<Vector3> _steps;

    [SerializeField] private int range = 10;

    private int _stepId = 1;

    private bool goal = false;
    //leads player to the exit. Limited steps. Starts when player enters the room
    // Start is called before the first frame update
    void Start() {}
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
            var tmp = Map.Escape (new Vector3Int (StartPos.x, StartPos.z));
            foreach (var pos in tmp)
            {
                _steps.Add (new Vector3 (pos.x + 0.5f, 0.5f, pos.y + 0.5f) * scale);
            }

            Ready = true;
        }
    }
    
    // Update is called once per frame
    void Update() {
        if (Ready)
        {
            if (Input.GetKeyDown (KeyCode.K))//zasadniczy warunek: jeżeli gracz jest w tym samym pokoju
            {
                if (_stepId <= range && goal == false)
                {
                    goal = true;
                    transform.rotation = Quaternion.LookRotation (_steps[_stepId]-transform.position);
                }
            }
            Move ();
        }
       
    }

    void Move() {
        if (goal)
        {
            Swim ();
            transform.position = Vector3.MoveTowards (transform.position, _steps[_stepId], speed*Time.deltaTime);
            if (transform.position == _steps[_stepId])
            {
                _stepId++;
                goal = false;
                CalcStarPos = transform.position;
            }
        }
        else
        {
            Circle ();
        }
    }
}
