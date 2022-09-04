using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFish : FishGeneral
{
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
    }
    
    // Update is called once per frame
    void Update() {
        Circle ();
    }
}
