using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ViewCtrlUIBase : MonoBehaviour
{
    #region 配置

    //在ui管理类中，加载prefab后设置
    public string prefabPath = "";

    public UILayer layer = UILayer.Invalid;

    //关闭按钮时 是否立即销毁
    public bool bDestroyWhenClose = false;

    //常驻界面
    public bool bResident = false;

    #endregion

    private List<int> m_eventIDList = new List<int>();


    #region 周期函数

    private void Awake()
    {
        this.onAwake();
    }

    private void Start()
    {
        this.onStart();
    }

    private void OnEnable()
    {
        this.onEnable();
    }

    private void OnDisable()
    {
        this.onDisable();
    }

    private void OnDestroy()
    {
        if (m_adapterList != null)
        {
            for (int i = 0; i < m_adapterList.Count; i++)
            {
                m_adapterList[i].Dispose();
            }

            m_adapterList.Clear();
        }

        EventManager.Instance.OffAll(this);

        this.onDestroy();
    }

    #endregion


    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnPause()
    {
        // CanvasGroup.blocksRaycasts = false;
        // gameObject.SetActive(false); //不能false，view会被pop隐藏
    }

    public virtual void OnResume()
    {
        // gameObject.SetActive(true);
        // CanvasGroup.blocksRaycasts = true;
    }

    public virtual void OnExit()
    {
        CancelInvoke();
        gameObject.SetActive(false);
    }


    public virtual void OnHide()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnClose()
    {
        // uiAdapter.HideUI();
        gameObject.SetActive(false);
    }

    public virtual void onAwake()
    {
    }

    public virtual void onStart()
    {
    }

    public virtual void onEnable()
    {
    }

    public virtual void onDisable()
    {
    }

    public virtual void onDestroy()
    {
    }

    public void OnButtonClick(Button btn, UnityAction onClicked, AudioType type = AudioType.none)
    {
        if (btn == null)
        {
            Debug.LogError("btn 为 null");
            return;
        }

        btn.onClick.AddListener(() =>
        {
            //播放音效
            if (type != AudioType.none)
            {
                audioAdapter.PlayAudio(type);
            }

            onClicked();
        });
    }

    #region 适配器

    private List<BaseAdapter> m_adapterList;

    UIAdapter m_uiAdapter;

    public UIAdapter uiAdapter
    {
        get
        {
            if (m_uiAdapter == null)
            {
                m_uiAdapter = CreateAdapter<UIAdapter>();
            }

            return m_uiAdapter;
        }
    }

    ResAdapter m_resAdapter;

    public ResAdapter resAdapter
    {
        get
        {
            if (m_resAdapter == null)
            {
                m_resAdapter = CreateAdapter<ResAdapter>();
            }

            return m_resAdapter;
        }
    }

    AudioAdapter m_audioAdapter;

    public AudioAdapter audioAdapter
    {
        get
        {
            if (m_audioAdapter == null)
            {
                m_audioAdapter = CreateAdapter<AudioAdapter>();
            }

            return m_audioAdapter;
        }
    }

    private TA CreateAdapter<TA>() where TA : BaseAdapter, new()
    {
        if (m_adapterList == null)
            m_adapterList = new List<BaseAdapter>();
        var t = new TA();
        m_adapterList.Add(t);
        return t;
    }

    #endregion
}