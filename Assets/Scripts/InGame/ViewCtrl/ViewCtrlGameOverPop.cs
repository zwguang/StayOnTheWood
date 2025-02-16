using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlGameOverPop : MonoBehaviour
{
    [SerializeField]
    private Button m_restartBtn;

    private void Awake()
    {
        m_restartBtn.onClick.AddListener(OnRestartBtnClicked);
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
        this.gameObject.SetActive(false);
        EventManager.Instance.Trigger((int)E.GameStart);
    }
}
