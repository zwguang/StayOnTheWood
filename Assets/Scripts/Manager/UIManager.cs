using System.Collections.Generic;
using Game;
using GDK;
using UnityEngine;
using UnityEngine.UI;

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
    private Dictionary<string, ViewCtrlBase> m_panelActiveDict = new Dictionary<string, ViewCtrlBase>(); //显示的面板
    private Dictionary<string, ViewCtrlBase> m_panelHideDict = new Dictionary<string, ViewCtrlBase>(); //隐藏的面板

    private List<KeyValuePair<string, ViewCtrlBase>> m_listTemp = new List<KeyValuePair<string, ViewCtrlBase>>();

    List<ViewCtrlBase> _tipsList = new List<ViewCtrlBase>(); //todo tips该怎么处理

    public Transform popLayer = null;
    public Transform menuLayer = null;
    public Transform viewLayer = null;
    public Transform tipsLayer = null;
    public Transform transitionLayer = null;
    public Transform handleLayer = null;
    public Transform guideLayer = null;
    public Transform topLayer = null;
    public Transform parent = null;


    private ViewCtrlBase CreatePanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        var prefab = ResManager.Instance.Load<GameObject>(prefabPath);
        GameObject panelObj = GameObject.Instantiate(prefab) as GameObject;
        var viewCtrl = panelObj.GetComponent<ViewCtrlBase>();
        viewCtrl.prefabPath = prefabPath;

        //添加canvas
        var canvas = viewCtrl.GetComponent<Canvas>();
        if (!canvas)
        {
            viewCtrl.gameObject.AddComponent<Canvas>();
        }

        var gr = viewCtrl.GetComponent<GraphicRaycaster>();
        if (!gr)
        {
            viewCtrl.gameObject.AddComponent<GraphicRaycaster>();
        }

        //设置order

        panelObj.transform.SetParent(GetParent(viewCtrl.layer), false);
        if (viewCtrl.layer == UILayer.Tips)
        {
            _tipsList.Add(viewCtrl);
        }
        else
        {
            m_panelActiveDict.Add(prefabPath, viewCtrl);
        }

        callBack?.Invoke(prefabPath, prefab);

        return viewCtrl;
    }

    private ViewCtrlBase GetPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        ViewCtrlBase viewCtrl = null;
        //从显示界面找
        if (this.m_panelActiveDict.ContainsKey(prefabPath))
        {
            viewCtrl = this.m_panelActiveDict.GetValue(prefabPath);
            viewCtrl.transform.SetAsFirstSibling();
            return viewCtrl;
        }

        //从隐藏界面里找
        if (this.m_panelHideDict.ContainsKey(prefabPath))
        {
            viewCtrl = this.m_panelHideDict.GetValue(prefabPath);
            this.m_panelHideDict.Remove(prefabPath);
            this.m_panelActiveDict.Add(prefabPath, viewCtrl);
            viewCtrl.gameObject.SetActive(true);
            return viewCtrl;
        }

        return null;
    }

    /// <summary>
    /// 页面入栈，显示在界面上
    /// 针对view、menu、pop有不同的逻辑处理
    /// </summary>
    public ViewCtrlBase ShowPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        ViewCtrlBase viewCtrl = this.GetPanel(prefabPath, callBack);
        if (viewCtrl == null)
        {
            //创建一个
            viewCtrl = this.CreatePanel(prefabPath, callBack);
        }

        return viewCtrl;
    }


    /// <summary>
    /// 移除页面, 如果菜单界面夹杂在中间位置，可能无法触发resume
    /// </summary>
    public void HidePanel(string prefabPath)
    {
        if (this.m_panelActiveDict.ContainsKey(prefabPath))
        {
            var vc = this.m_panelActiveDict.GetValue(prefabPath);
            this.m_panelActiveDict.Remove(prefabPath);
            this.m_panelHideDict.Add(prefabPath, vc);
            vc.gameObject.SetActive(false);
        }
        else if (this.m_panelHideDict.ContainsKey(prefabPath))
        {
            SDebug.Log($"资源已经被隐藏 prefabPath = {prefabPath}");
        }
        else
        {
            SDebug.Log($"资源不存在 prefabPath = {prefabPath}");
        }
    }


    public ViewCtrlBase ShowTips(string prefabPath)
    {
        ViewCtrlBase panel = null;
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
    public void ClearAllWithoutResident()
    {
        ClearAllTips();

        this.m_listTemp.Clear();
        foreach (var var in m_panelActiveDict)
        {
            if (!var.Value.bResident)
            {
                this.m_listTemp.Add(var);
            }
        }

        foreach (var var in m_panelHideDict)
        {
            if (!var.Value.bResident)
            {
                this.m_listTemp.Add(var);
            }
        }

        for (int i = 0; i < m_listTemp.Count; i++)
        {
            GameObject.Destroy(m_listTemp[i].Value.gameObject);

            var key = m_listTemp[i].Key;
            if (m_panelActiveDict.ContainsKey(key))
            {
                m_panelActiveDict.Remove(key);
            }

            if (m_panelHideDict.ContainsKey(key))
            {
                m_panelHideDict.Remove(key);
            }
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