using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameStatus gameStatus;

    public GameStatus GameStatus
    {
        private set
        {
            gameStatus = value;
            gameStatus.TriggerEventData(DataPath.GAME_STATUS);
        }
        get { return gameStatus; }
    }
    private void Start()
    {
        GameStatus = GameStatus.InMainMenu;
    }

    public void OnEndGame()
    {
        GameStatus = GameStatus.EndGame;
    }
    public void OnStartGame()
    {
        GameStatus = GameStatus.StartGame;
    }
    public void OnBackHome()
    {
        GameStatus = GameStatus.InMainMenu;
    }
}

public enum GameStatus
{
    InMainMenu,
    StartGame,
    EndGame,
}