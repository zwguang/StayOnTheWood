using System.Collections.Generic;
using Game;
using GDK;
using UnityEditor.UIElements;
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
    private Dictionary<string, UIBase> m_uiDict = new Dictionary<string, UIBase>(); //显示的面板

    //隐藏的面板,头插法,维护15个
    private List<UIBase> m_uiHideList = new List<UIBase>(15);

    private List<KeyValuePair<string, UIBase>> m_listTemp = new List<KeyValuePair<string, UIBase>>();

    List<UIBase> _tipsList = new List<UIBase>(); //todo tips该怎么处理

    public Transform popLayer = null;
    public Transform menuLayer = null;
    public Transform viewLayer = null;
    public Transform tipsLayer = null;
    public Transform transitionLayer = null;
    public Transform handleLayer = null;
    public Transform guideLayer = null;
    public Transform topLayer = null;
    public Transform parent = null;


    private UIBase CreatePanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        var prefab = ResManager.Instance.Load<GameObject>(prefabPath);
        GameObject panelObj = GameObject.Instantiate(prefab) as GameObject;
        var viewCtrl = panelObj.GetComponent<UIBase>();
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
            m_uiDict.Add(prefabPath, viewCtrl);
        }

        callBack?.Invoke(prefabPath, prefab);

        return viewCtrl;
    }

    private UIBase GetPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        UIBase ui = this.m_uiDict.GetValue(prefabPath);
        if (ui != null)
        {
            ui.transform.SetAsFirstSibling();
            //从隐藏列表中拿出来
            if (!ui.gameObject.activeSelf)
            {
                ui.gameObject.SetActive(true);
                this.m_uiHideList.Remove(ui);
            }
        }

        return ui;
    }

    /// <summary>
    /// 页面入栈，显示在界面上
    /// 针对view、menu、pop有不同的逻辑处理
    /// </summary>
    public UIBase ShowPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        UIBase ui = this.GetPanel(prefabPath, callBack);
        if (ui == null)
        {
            //创建一个
            ui = this.CreatePanel(prefabPath, callBack);
        }

        return ui;
    }


    /// <summary>
    /// 移除页面, 如果菜单界面夹杂在中间位置，可能无法触发resume
    /// </summary>
    public void HidePanel(string prefabPath)
    {
        var ui = this.m_uiDict.GetValue(prefabPath);
        if (ui)
        {
            ui.gameObject.SetActive(false);
            //缓存满了
            if (this.m_uiHideList.Count == this.m_uiHideList.Capacity)
            {
                var tempUI = m_uiHideList.Pop();
                this.m_uiDict.Remove(tempUI.prefabPath);
                SDebug.Log($"触发LRU,移除冗余面板 prefabPath = {prefabPath}");
            }

            this.m_uiHideList.Insert(0, ui);
        }
        else
        {
            SDebug.Log($"资源不存在 prefabPath = {prefabPath}");
        }
    }


    public UIBase ShowTips(string prefabPath)
    {
        UIBase panel = null;
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