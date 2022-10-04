using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFish : FishGeneral
{
    // circles + points if in the same room
    // NIETESTOWANE
    [SerializeField] private PlayerMechanics player;
    [SerializeField] private int value;
    private bool _found = false;

    void Start()
    {

    }
    protected void Awake()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChanged;
    }

    protected void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChanged;
    }

    new void GameManagerOnGameStateChanged(GameState state)
    {
        base.GameManagerOnGameStateChanged(state);
        if (state == GameState.Game)
        {
            Ready = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Ready)
        {
            if (!_found && player.GetPosFixed() == StartPosFixed)//zasadniczy warunek: jeżeli gracz jest w tym samym pokoju
            //Działa, ale nie idealnie, wymaga przepłynięcia dokładnie przez środek pokoju
            //Poprawki trzeba zrobić w kodzie liczącym pozycję łodzi
            {
                _found = true;
            }

            if (_found)
            {
                var pl = player.GetPosRelative();
                speed *= 2;
                if (transform.position == pl)
                {
                    player.ChangePoints(value);
                    Destroy(this);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, pl, speed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(pl - transform.position) * Quaternion.Euler(0, 180, 0);
                }
            }
            else
            {
                Circle();
            }

        }
    }
}
