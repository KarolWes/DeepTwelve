using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFish : FishGeneral
{
    //light + pointless wander
    // NIETESTOWANE
    private Vector3 _finish;
    protected void Awake() {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    protected void OnDestroy() {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    new void GameManagerOnGameStateChanged(GameState state) {
        base.GameManagerOnGameStateChanged (state);
        if (state == GameState.Game)
        {
            Ready = true;
        }
    }

    // Update is called once per frame
    void Update() {
        Move ();
    }

    void Move() {
        SwimAnimation ();
        transform.position = Vector3.MoveTowards (transform.position, _finish, speed*Time.deltaTime);
        if (transform.position == _finish)
        {
            var step = GetNextStep ();
            _finish = new Vector3 (step.x, Random.Range (0.0f, 1.0f)*MapManager.Scale, step.y);
            transform.rotation = Quaternion.LookRotation (_finish-transform.position)*Quaternion.Euler (0,180,0);
        }
        
    }

    private Vector3 GetNextStep() {
        var pos = Vector3Int.FloorToInt (transform.position / MapManager.Scale - new Vector3 (0.5f, 0.5f, 0.5f));
        var acc = Map.GetAccessible (pos);
        var next = new List<Vector3Int> ();
        for (int j = 0; j < 4; j ++)
        {
            if (!acc[j])
            {
                next.Add (new Vector3Int (pos.x+Map.Neighbours[j,0], pos.y+Map.Neighbours[j,1], 0));
            }
        }
        return next[Random.Range (0, next.Count)];
    }
}
