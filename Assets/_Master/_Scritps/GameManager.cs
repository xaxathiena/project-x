using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SpawnEndlessControl spawnEndlessControl;
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
        StartCoroutine(SetTargetFPS());
    }

    private IEnumerator SetTargetFPS()
    {
        yield return null;
        Application.targetFrameRate = 60;
    }
    public void OnEndGame()
    {
        spawnEndlessControl.isSpawn = false;
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