using System.Collections.Generic;
using GDK;
using UnityEngine;

namespace GDK
{
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

    public class UIAdapter : BaseAdapter
    {
        Dictionary<string, UIBase> m_uiDic = new Dictionary<string, UIBase>();

        public delegate void CreateCallBack(string path, GameObject go);


        protected override void OnInvalid()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDispose()
        {
            throw new System.NotImplementedException();
        }

        public UIBase ShowPanel(string path)
        {
            return SystemUIManager.Instance.ShowPanel(path);
        }

        public void HidePanel(string path)
        {
            SystemUIManager.Instance.HidePanel(path);
        }
    }
}