using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject homeMenuUI;
    [SerializeField] private GameObject ingameResultUI;

    private void Awake()
    {
        DataTrigger.RegisterValueChange(DataPath.GAME_STATUS, (value) =>
        {
            GameStatus status = (GameStatus)value;
            Debug.LogWarning("status = " + status);
            homeMenuUI.SetActive(status == GameStatus.InMainMenu);
            ingameUI.SetActive(status == GameStatus.StartGame);
            ingameResultUI.SetActive(status == GameStatus.EndGame);
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
}
