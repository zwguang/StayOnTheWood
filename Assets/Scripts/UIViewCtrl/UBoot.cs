using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using TMPro;
using UnityEngine;

public class UBoot : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_scoreTmp;

    [SerializeField] private GameObject m_gameOverPopGo;
    private void Awake()
    {
        EventManager.Instance.On((int)E.PlayerScore, OnPlayerSore);
        EventManager.Instance.On((int)E.PlayerDeath, OnPlayerDeath);
        EventManager.Instance.On((int)E.GameStart, OnGameStart);
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
}
