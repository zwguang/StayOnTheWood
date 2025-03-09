using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameMainPanel : UIBottomBase
{
    [SerializeField] private TextMeshProUGUI m_scoreTmp;

    [SerializeField] private Button m_homeBtn;

    public override void onAwake()
    {
        SystemEventManager.Instance.On((int)EventID.PlayerScore, OnPlayerSore);
        SystemEventManager.Instance.On((int)EventID.PlayerDeath, OnPlayerDeath);
        SystemEventManager.Instance.On((int)EventID.GameStart, OnGameStart);

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
        UIGameOverPop.OpenUIPanel();
    }

    void OnHomeBtnClicked()
    {
        SystemSceneManager.Instance.ChangeSceneByName("HallScene");
    }
}