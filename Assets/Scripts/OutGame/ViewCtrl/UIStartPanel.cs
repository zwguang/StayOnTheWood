using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStartPanel : UIBottomBase
{
    [SerializeField] private Button m_startBtn;
    [SerializeField] private Button m_codeScanBtn;
    [SerializeField] private Button m_copyBtn;
    [SerializeField] private TextMeshProUGUI m_copyTmp;

    public override void onAwake()
    {
        SDebug.Log("ViewCtrlStartPanel onAwake");
        // OnButtonClick(m_startBtn, OnStartBtnCLicked);
        m_startBtn.onClick.AddListener(OnStartBtnCLicked);
        m_codeScanBtn.onClick.AddListener(OnCodeScanBtnCLicked);
        m_copyBtn.onClick.AddListener(OnCopyBtnClicked);
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

    void OnCodeScanBtnCLicked()
    {
        UIManager.Instance.ShowPanel(ResPath.prefabPath_QRCodeScan);
    }

    void OnCopyBtnClicked()
    {
        GUIUtility.systemCopyBuffer = "说你又不听，听你又不做~";
        m_copyTmp.text = GUIUtility.systemCopyBuffer;
    }
}