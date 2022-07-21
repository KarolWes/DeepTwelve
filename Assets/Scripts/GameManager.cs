using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChange;
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.MapCreation);
    }
    
    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MapCreation:
                break;
            case GameState.Game:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        State = newState;
        OnGameStateChange?.Invoke(newState);
    }
}

public enum GameState {
    MapCreation,
    Game
}