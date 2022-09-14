using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class BlueFish : FishGeneral {
    [SerializeField] private int range = 10;
    [SerializeField] private PlayerMechanics player;
    private int _stepId = 1;
    private List<Vector3> _steps;
    private bool _goal = false;
    
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
            var tmp = Map.Escape (new Vector3Int (StartPosFixed.x, StartPosFixed.z));
            foreach (var pos in tmp)
            {
                _steps.Add (new Vector3 (pos.x + 0.5f, 0.5f, pos.y + 0.5f) * MapManager.Scale);
            }

            Ready = true;
        }
    }
    
    // Update is called once per frame
    void Update() {
        //Debug.Log ("Fish: " + StartPosFixed );
        if (Ready)
        {
            if (player.GetPosFixed () == StartPosFixed)//zasadniczy warunek: jeżeli gracz jest w tym samym pokoju
                                                       //Działa, ale nie idealnie, wymaga przepłynięcia dokładnie przez środek pokoju
                                                       //Poprawki trzeba zrobić w kodzie liczącym pozycję łodzi
            {
                if (_stepId <= range && _goal == false)
                {
                    _goal = true;
                    transform.rotation = Quaternion.LookRotation (_steps[_stepId]-transform.position);
                }
            }
            Move ();
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
