using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlStartPanel : ViewCtrlUIBase
{
    [SerializeField] private Button m_startBtn;

    public override void onAwake()
    {
        SDebug.Log("ViewCtrlStartPanel onAwake");
        // OnButtonClick(m_startBtn, OnStartBtnCLicked);
        m_startBtn.onClick.AddListener(OnStartBtnCLicked);
    }

    void OnStartBtnCLicked()
    {
        SDebug.Log("ViewCtrlStartPanel OnStartBtnCLicked");

        UIManager.Instance.HidePanel();
        GameSceneManager.Instance.ChangeSceneByName("InGameScene");
    }
}