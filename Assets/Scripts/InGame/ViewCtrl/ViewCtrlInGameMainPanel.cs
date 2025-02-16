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
        EventManager.Instance.On((int)E.PlayerScore, OnPlayerSore);
        EventManager.Instance.On((int)E.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.On((int)E.GameStart, OnGameStart);
        
        m_homeBtn.onClick.AddListener(OnHomeBtnClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
