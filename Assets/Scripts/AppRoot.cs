using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;

public class AppRoot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //1. APP的基本配置
        InitApp();

        //资源解压

        //热更检测

        //初始化系统
        InitSystem();

        //闪屏
        Splash();
    }

    private void OnDestroy()
    {
        UIManager.Instance.ClearAllWithoutResident();
    }

    void InitApp()
    {
        //关闭多点触摸
        Input.multiTouchEnabled = false;

        Application.targetFrameRate = 30;
        Application.runInBackground = true;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Debug.Log($"InitUnity default quality:{QualitySettings.GetQualityLevel()}");
        QualitySettings.SetQualityLevel(4);

        DontDestroyOnLoad(this);

        // Instantiate(Resources.Load("Prefabs/UIRoot"));

        GameSceneManager.Instance.ChangeSceneByName("HallScene");

        // UIManager.Instance.ShowPanel(ResPath.prefabPath_StartPanel);
    }

    void InitSystem()
    {
    }

    void Splash()
    {
    }
}