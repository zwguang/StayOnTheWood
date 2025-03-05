using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlGameOverPop : ViewCtrlBase
{
    [SerializeField] private Button m_restartBtn;

    public override void onAwake()
    {
        OnButtonClick(m_restartBtn, OnRestartBtnClicked);
    }

    public static void OpenUIPanel()
    {
        UIManager.Instance.ShowPanel(ResPath.prefabPath_GameOverPop);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnRestartBtnClicked()
    {
        UIManager.Instance.HidePanel(prefabPath);
        EventManager.Instance.Trigger((int)EventID.GameStart);
    }
}