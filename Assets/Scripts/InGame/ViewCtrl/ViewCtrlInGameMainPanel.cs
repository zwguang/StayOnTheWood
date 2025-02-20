using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlInGameMainPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_scoreTmp;

    [SerializeField] private GameObject m_gameOverPopGo;
    [SerializeField] private Button m_homeBtn;
    private void Awake()
    {
        EventManager.Instance.On((int)EventID.PlayerScore, OnPlayerSore);
        EventManager.Instance.On((int)EventID.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.On((int)EventID.GameStart, OnGameStart);
        
        m_homeBtn.onClick.AddListener(OnHomeBtnClicked);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Off((int)EventID.PlayerScore, OnPlayerSore);
        EventManager.Instance.Off((int)EventID.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.Off((int)EventID.GameStart, OnGameStart);
    }

    private void OnGameStart()
    {
        this.m_scoreTmp.text = $"得分：0";

    }
    private void OnPlayerSore()
    {
        this.m_scoreTmp.text = $"得分：{GameManager.Instance.soreNum.ToString()}";
    }

    private void OnPlayerDeath()
    {
        m_gameOverPopGo.SetActive(true);
    }

    void OnHomeBtnClicked()
    {
        GameSceneManager.Instance.ChangeSceneByName("HomeScene");
    }
}
