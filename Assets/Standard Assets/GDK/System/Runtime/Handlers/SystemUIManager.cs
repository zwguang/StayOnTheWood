using System;
using System.Collections.Generic;
using System.Linq;
using GDK;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class SystemUIManager : Singleton<SystemUIManager>
{
    private Dictionary<string, UIBase> m_uiDict = new Dictionary<string, UIBase>(); //所有的面板

    //隐藏的面板,头插法,维护15个
    private List<UIBase> m_uiHideList = new List<UIBase>(10);

    private List<KeyValuePair<string, UIBase>> m_listTemp = new List<KeyValuePair<string, UIBase>>();

    List<UIBase> _tipsList = new List<UIBase>(); //todo tips该怎么处理

    //界面order间隔250
    private static int SORTINGORDER_VIEW_GAP = 250;

    //一个layer上最多显示20个界面
    private static int UILAYER_VIEW_MAX = 20;

    //layer的order的间隔
    private static int UILAYER_GAP = UILAYER_VIEW_MAX * SORTINGORDER_VIEW_GAP;

    //每个layer最开始的sortorder
    private int[] UILAYER_START_SORT_ORDER = new int[6]
    {
        0,
        UILAYER_GAP * 1,
        UILAYER_GAP * 2,
        UILAYER_GAP * 3,
        UILAYER_GAP * 4,
        UILAYER_GAP * 5
    };

    protected override void OnConstruct()
    {
        base.OnConstruct();

        int maxSortOrder = UILAYER_START_SORT_ORDER.Last() + UILAYER_GAP;
        SDebug.Assert(maxSortOrder < Int16.MaxValue, "sortorder最大值不能超过 Int16.MaxValue");
    }

    private UIBase CreatePanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        var prefab = SystemResManager.Instance.Load<GameObject>(prefabPath);
        GameObject panelObj = GameObject.Instantiate(prefab) as GameObject;
        var viewCtrl = panelObj.GetComponent<UIBase>();
        viewCtrl.prefabPath = prefabPath;

        UIRoot.Instance.AddToParent(panelObj, viewCtrl.layer);

        //添加canvas
        AddCanvas(viewCtrl);
        //设置order
        SetOrderAndMask(viewCtrl);

        m_uiDict.Add(prefabPath, viewCtrl);
        callBack?.Invoke(prefabPath, prefab);

        return viewCtrl;
    }

    private void AddCanvas(UIBase ui)
    {
        var canvas = ui.GetComponent<Canvas>();
        if (!canvas)
        {
            canvas = ui.gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
        }

        var gr = ui.GetComponent<GraphicRaycaster>();
        if (!gr)
        {
            ui.gameObject.AddComponent<GraphicRaycaster>();
        }
    }

    private void SetOrderAndMask(UIBase ui)
    {
        int layerOrder = this.UILAYER_START_SORT_ORDER[(int)ui.layer];

        var canvas = ui.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = layerOrder;

        ui.SetMask();
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

    public UIBase ShowPanel(string prefabPath, UIAdapter.CreateCallBack callBack = null)
    {
        SDebug.Log($"ShowPanel prefabPath = {prefabPath}");

        UIBase ui = this.GetPanel(prefabPath, callBack);
        if (ui == null)
        {
            // SDebug.Log($"CreatePanel prefabPath = {prefabPath}");
            //创建一个
            ui = this.CreatePanel(prefabPath, callBack);
        }

        ui.gameObject.SetActive(true);
        return ui;
    }


    /// <summary>
    /// 移除页面,
    /// </summary>
    public void HidePanel(string prefabPath)
    {
        SDebug.Log($"HidePanel prefabPath = {prefabPath}");

        var ui = this.m_uiDict.GetValue(prefabPath);
        if (ui)
        {
            ui.gameObject.SetActive(false);
            //缓存满了
            if (this.m_uiHideList.Count >= this.m_uiHideList.Capacity)
            {
                //如果自己在缓存中
                if (this.m_uiHideList.Contains(ui))
                {
                    this.m_uiHideList.Remove(ui);
                    this.m_uiHideList.Insert(0, ui);
                }
                else
                {
                    var tempUI = m_uiHideList.Pop();
                    this.m_uiDict.Remove(tempUI.prefabPath);
                    GameObject.Destroy(tempUI.gameObject);
                    SDebug.Log($"触发LRU,销毁冗余面板 prefabPath = {tempUI.prefabPath}");
                }
            }

            this.m_uiHideList.Insert(0, ui);
        }
        else
        {
            SDebug.LogError($"资源不存在 prefabPath = {prefabPath}");
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
}