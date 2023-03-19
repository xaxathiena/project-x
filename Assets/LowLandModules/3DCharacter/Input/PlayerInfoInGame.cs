using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PlayerInfoInGame : MonoBehaviour
{
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI enemyDead;
    [SerializeField] private TextMeshProUGUI enemyAlive;
    [SerializeField] private Image currentExpImg;
    [SerializeField] private Image currentHealthPlayerImage;
    private int scorePerEnemy = 100;
    private float currentPlayerExp;
    private float nextPlayerLevelExp;
    private float currentHealPlayer;
    private float currentMaxHealPlayer;
    private int currentScore;
    private void Awake()
    {
        DataTrigger.RegisterValueChange(DataPath.INGAME_ENEMY_DEAD, (value) =>
        {
            enemyDead.text = value.ToString();
            score.text = (((int)value) * scorePerEnemy).ToString();
        });
        
        DataTrigger.RegisterValueChange(DataPath.INGAME_ENEMY_ALIVE, (value) =>
        {
            enemyAlive.text = value.ToString();
        });
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_UPLEVEL, (value) =>
        {
            level.text = value.ToString();
        });
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_NEXT_EXP, (value) =>
        {
            nextPlayerLevelExp = (float)value;
            SetExpUI();
        });
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_EXP, (value) =>
        {
            currentPlayerExp = (float)value;
            SetExpUI();
        });
        
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_HEAL, (value) =>
        {
            currentHealPlayer = (float)value;
            SetHealUI();
        });
        DataTrigger.RegisterValueChange(DataPath.INGAME_PLAYER_MAX_HEAL, (value) =>
        {
            currentMaxHealPlayer = (float)value;
            SetHealUI();
        });
    }

    private void SetExpUI()
    {
        currentExpImg.fillAmount = currentPlayerExp * 1f / (nextPlayerLevelExp * 1f);
    }
    private void SetHealUI()
    {
        currentHealthPlayerImage.color =  colorGradient.Evaluate(1- currentHealthPlayerImage.fillAmount);
        currentHealthPlayerImage.fillAmount = currentHealPlayer * 1f / (currentMaxHealPlayer * 1f);
    }
}
