using System;
using System.Collections;
using System.Collections.Generic;
using GDK;
using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private static UIRoot m_instance = null;

    public static UIRoot Instance
    {
        get { return m_instance; }
    }

    public Transform bottomLayer = null;
    public Transform menuLayer = null;
    public Transform popLayer = null;
    public Transform tipsLayer = null;
    public Transform dialogLayer = null;
    public Transform guideLayer = null;
    public Canvas canvas = null;

    private UIRoot()
    {
    }

    private void Awake()
    {
        m_instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AddToParent(GameObject go, UILayer layer)
    {
        go.transform.SetParent(GetParent(layer), false);
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