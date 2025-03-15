using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverPop : UIPopBase
{
    [SerializeField] private Button m_restartBtn;

    public override void onAwake()
    {
        base.onAwake();
        OnButtonClick(m_restartBtn, OnRestartBtnClicked);
    }

    public static void OpenUIPanel()
    {
        SystemUIManager.Instance.ShowPanel(ResPath.prefabPath_GameOverPop);
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
        SystemUIManager.Instance.HidePanel(prefabPath);
        SystemEventManager.Instance.Trigger((int)EventID.GameStart);
    }
}