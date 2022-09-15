using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFish : FishGeneral
{
    //The agrresive one. circle + follows player, if too many near player some fine
    //NIETESTOWANE
    [SerializeField] private PlayerMechanics player;
    private bool _found = false;
    void Start()
    {
        
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

    // Update is called once per frame
    void Update()
    {
        if ( !_found && player.GetPosFixed () == StartPosFixed)//zasadniczy warunek: jeżeli gracz jest w tym samym pokoju
        //Działa, ale nie idealnie, wymaga przepłynięcia dokładnie przez środek pokoju
        //Poprawki trzeba zrobić w kodzie liczącym pozycję łodzi
        {
            _found = true;
            if (player.ChangeShoalCount ())
            {
                Destroy (this);
            }
        }
        if (_found)
        {
            var pl = player.GetPosRelative () + new Vector3 (Random.Range (0.2f, MapManager.Scale/2.0f), Random.Range (0.2f, MapManager.Scale/2.0f),Random.Range (0.2f, MapManager.Scale/2.0f));
            transform.position = Vector3.MoveTowards (transform.position, pl, speed*Time.deltaTime);
            transform.rotation = Quaternion.LookRotation (pl-transform.position)*Quaternion.Euler (0,180,0);
        }
        else
        {
            Circle ();
        }
    }
}
