using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class UIStartPanel : UIBase
{
    [SerializeField] private Button m_startBtn;

    public override void onAwake()
    {
        SDebug.Log("ViewCtrlStartPanel onAwake");
        // OnButtonClick(m_startBtn, OnStartBtnCLicked);
        m_startBtn.onClick.AddListener(OnStartBtnCLicked);
    }

    public static void OpenUIPanel()
    {
        SystemUIManager.Instance.ShowPanel(ResPath.prefabPath_StartPanel);
    }

    void OnStartBtnCLicked()
    {
        SDebug.Log("ViewCtrlStartPanel OnStartBtnCLicked");

        SystemUIManager.Instance.HidePanel(prefabPath);
        SystemSceneManager.Instance.ChangeSceneByName("GameScene");
    }
}