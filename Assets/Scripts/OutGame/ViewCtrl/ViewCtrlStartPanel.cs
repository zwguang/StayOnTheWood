using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlStartPanel : MonoBehaviour
{
    [SerializeField]
    private Button m_startBtn;

    private void Awake()
    {
        m_startBtn.onClick.AddListener(OnStartBtnCLicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStartBtnCLicked()
    {
        GameSceneManager.Instance.ChangeSceneByName("InGameScene");
    }
}
