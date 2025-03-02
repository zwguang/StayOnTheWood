using System.Collections.Generic;
using GDK;
using UnityEngine;

public class UIAdapter : BaseAdapter
{
    Dictionary<string, string> m_pathDic = new Dictionary<string, string>();

    public delegate void CreateCallBack(string path, GameObject go);

    
    protected override void OnInvalid()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDispose()
    {
        throw new System.NotImplementedException();
    }
}