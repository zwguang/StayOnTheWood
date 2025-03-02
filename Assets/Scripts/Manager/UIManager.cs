using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;

public enum UILayer
{
    //UI最底层，再下面就是3D世界了
    Bottom,

    //菜单层
    Menu,

    //弹出面板层，会有个mask蒙版
    Pop,
    Tips,

    Dialog,

    //新手引导层
    Guide,
    Invalid
}

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, ViewCtrlUIBase> m_panelDict = new Dictionary<string, ViewCtrlUIBase>(); //存储所有面板
    private List<ViewCtrlUIBase> m_panelStack = new List<ViewCtrlUIBase>(); //管理所有显示的面板

    private string m_curPrefabPath;
    List<ViewCtrlUIBase> _tipsList = new List<ViewCtrlUIBase>();

    public Transform popLayer = null;
    public Transform menuLayer = null;
    public Transform viewLayer = null;
    public Transform tipsLayer = null;
    public Transform transitionLayer = null;
    public Transform handleLayer = null;
    public Transform guideLayer = null;
    public Transform topLayer = null;
    public Transform parent = null;


    List<ViewCtrlUIBase> GetPanelStack(string prefabPath)
    {
        // var panelStack = _panelStackDic.GetValue(prefabPath.layer);
        // if (panelStack == null)
        // {
        //     panelStack = new List<ViewCtrlUIBase>();
        //     _panelStackDic[prefabPath.layer] = panelStack;
        // }

        return m_panelStack;
    }

    private ViewCtrlUIBase CreatePanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        var prefab = ResManager.Instance.Load<GameObject>(prefabPath);
        GameObject panelObj = GameObject.Instantiate(prefab) as GameObject;
        var viewCtrl = panelObj.GetComponent<ViewCtrlUIBase>();
        viewCtrl.prefabPath = prefabPath;
        panelObj.transform.SetParent(GetParent(viewCtrl.layer), false);
        if (viewCtrl.layer == UILayer.Tips)
        {
            _tipsList.Add(viewCtrl);
        }
        else
        {
            m_panelDict.Add(prefabPath, viewCtrl);
        }

        callBack?.Invoke(prefabPath, prefab);

        return viewCtrl;
    }

    /// <summary>
    /// 获取面板
    /// 1.如果存储panel的字典为空，则创建一个
    /// 2.从字典获取对应panel类型的panel
    /// 3.如果没获取到此类型的panel，则实例化一个，反之直接返回
    /// </summary>
    /// <param name="panelType"></param>
    private ViewCtrlUIBase GetPanel(string prefabPath, UIAdapter.CreateCallBack callBack, out bool bExist)
    {
        ViewCtrlUIBase panel = m_panelDict.GetValue(prefabPath);
        bExist = true;
        if (panel == null)
        {
            bExist = false;
            panel = CreatePanel(prefabPath, callBack);
        }

        return panel;
    }


    /// <summary>
    /// 页面入栈，显示在界面上
    /// 针对view、menu、pop有不同的逻辑处理
    /// </summary>
    public ViewCtrlUIBase ShowPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        var panelStack = GetPanelStack(prefabPath);

        //栈里有页面 暂停已有
        if (panelStack.Count > 0)
        {
            ViewCtrlUIBase topPanel = panelStack.Peek();

            //todo，弹窗出现会干掉view
            // if (topPanel.bDestroy)
            // {
            //     GameObject.Destroy(topPanel.gameObject);
            // }
            // else
            // {
            //     topPanel.OnPause();
            // }

            topPanel.OnPause();
        }

        bool bExist = true;

        //显示新页面
        ViewCtrlUIBase panel = GetPanel(prefabPath, callBack, out bExist);
        //已经存在的界面，重新整理在栈中的位置
        if (bExist)
        {
            panelStack.Remove(panel);
            panel.transform.SetAsLastSibling();
        }

        panelStack.Add(panel);
        m_curPrefabPath = prefabPath;

        panel.OnEnter();
        return panel;
    }


    /// <summary>
    /// 移除页面, 如果菜单界面夹杂在中间位置，可能无法触发resume
    /// </summary>
    public void HidePanel()
    {
        var panelStack = GetPanelStack(m_curPrefabPath);
        //弹出栈顶元素
        if (panelStack.Count > 0)
        {
            ViewCtrlUIBase topPanel = panelStack.Pop();
            if (topPanel.bDestroyWhenClose)
            {
                GameObject.Destroy(topPanel.gameObject);
                m_panelDict.Remove(m_curPrefabPath);
            }
            else
            {
                topPanel.OnExit();
            }
        }

        //顶部的第二个元素resume
        if (panelStack.Count > 0)
        {
            ViewCtrlUIBase topPanel = panelStack.Peek();
            topPanel.OnResume();
            m_curPrefabPath = topPanel.prefabPath;
        }
    }


    public ViewCtrlUIBase ShowTips(string prefabPath)
    {
        ViewCtrlUIBase panel = null;
        for (int i = 0; i < _tipsList.Count; i++)
        {
            if (!_tipsList[i].gameObject.activeSelf)
            {
                panel = _tipsList[i];
                panel.transform.SetAsLastSibling();
                break;
            }
        }

        if (panel == null)
        {
            panel = CreatePanel(prefabPath);
        }

        return panel;
    }

    //除了常驻界面，清除其他所有界面
    public void ClearAll()
    {
        ClearAllTips();

        m_panelStack.Clear();
        List<KeyValuePair<string, ViewCtrlUIBase>> list = new List<KeyValuePair<string, ViewCtrlUIBase>>();
        foreach (var var in m_panelDict)
        {
            if (!var.Value.bResident)
            {
                list.Add(var);
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            GameObject.Destroy(list[i].Value.gameObject);
            m_panelDict.Remove(list[i].Key);
        }
    }


    void ClearAllTips()
    {
        for (int i = 0; i < _tipsList.Count; i++)
        {
            GameObject.Destroy(_tipsList[i].gameObject);
        }

        _tipsList.Clear();
    }


    Transform GetParent(UILayer layer)
    {
        Transform parent = null;
        switch (layer)
        {
            case UILayer.Bottom:
            {
                parent = UIRoot.Instance.bottomLayer;
                break;
            }
            case UILayer.Menu:
            {
                parent = UIRoot.Instance.menuLayer;
                break;
            }
            case UILayer.Pop:
            {
                parent = UIRoot.Instance.popLayer;
                break;
            }
            case UILayer.Tips:
            {
                parent = UIRoot.Instance.tipsLayer;
                break;
            }
            case UILayer.Dialog:
            {
                parent = UIRoot.Instance.dialogLayer;
                break;
            }
            case UILayer.Guide:
            {
                parent = UIRoot.Instance.guideLayer;
                break;
            }
            default:
            {
                Debug.LogError("layer 异常");
                break;
            }
        }

        return parent;
    }
}