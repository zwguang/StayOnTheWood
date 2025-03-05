using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlInGameMainPanel : ViewCtrlUIBase
{
    [SerializeField] private TextMeshProUGUI m_scoreTmp;

    [SerializeField] private Button m_homeBtn;

    public override void onAwake()
    {
        EventManager.Instance.On((int)EventID.PlayerScore, OnPlayerSore);
        EventManager.Instance.On((int)EventID.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.On((int)EventID.GameStart, OnGameStart);

        OnButtonClick(m_homeBtn, OnHomeBtnClicked);
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
        ViewCtrlGameOverPop.OpenUIPanel();
    }

    void OnHomeBtnClicked()
    {
        UIManager.Instance.HidePanel();
        GameSceneManager.Instance.ChangeSceneByName("HallScene");
    }
}