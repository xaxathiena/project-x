using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;
public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject homeMenuUI;
    [SerializeField] private GameObject ingameResultUI;
    [SerializeField] private GameObject statsMonitorObj;
    [SerializeField] private Transform cameraMap;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI scoreTitle;
    [SerializeField] private TextMeshProUGUI currentHighScoreText;
    private int scorePerEnemy = 100;
    private int currentScore;
    private void Awake()
    {
        DataTrigger.RegisterValueChange(DataPath.GAME_STATUS, (value) =>
        {
            GameStatus status = (GameStatus)value;
            Debug.LogWarning("status = " + status);
            homeMenuUI.SetActive(status == GameStatus.InMainMenu);
            ingameUI.SetActive(status == GameStatus.StartGame);
            ingameResultUI.SetActive(status == GameStatus.EndGame);
            if (status == GameStatus.EndGame)
            {
                int currentHighScore = PlayerPrefs.GetInt(DataPath.HIGH_SCORE, 0);
                if (currentHighScore < currentScore)
                {
                    PlayerPrefs.SetInt(DataPath.HIGH_SCORE, currentScore);
                    scoreTitle.text = "New high score";
                }
                else
                {
                    scoreTitle.text = "High score";
                }

                score.text = currentScore.ToString();
            }
            else if (status== GameStatus.InMainMenu)
            {
                currentHighScoreText.text = PlayerPrefs.GetInt(DataPath.HIGH_SCORE, 0).ToString();
            }
        });
        DataTrigger.RegisterValueChange(DataPath.INGAME_ENEMY_DEAD, (value) =>
        {
            currentScore = ((int)value) * scorePerEnemy;
        });
    }

    public void OnStartGame()
    {
        GameManager.instance.OnStartGame();
    }

    public void OnReplay()
    {
        GameManager.instance.OnStartGame();
    }
    public void OnGoHome()
    {
        GameManager.instance.OnBackHome();
    }

    public void OnOffStatsMonitor()
    {
        statsMonitorObj.SetActive(!statsMonitorObj.activeSelf);
    }

    private void Update()
    {
        if (InGameManager.instance.CharacterSpawn != null)
        {
            cameraMap.position = InGameManager.instance.CharacterSpawn.position + Vector3.up * 10;
        }
    }
}
