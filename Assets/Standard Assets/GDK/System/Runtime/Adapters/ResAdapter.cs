using System;
using System.Collections.Generic;

namespace GDK
{
    
    public class ResAdapter : BaseAdapter
    {
        Dictionary<string, int> m_assetPathDic = new Dictionary<string, int>();

        protected override void OnDispose()
        {
            foreach (var v in m_assetPathDic)
            {
                ResManager.Instance.UnLoad(v.Key, v.Value);
            }
            m_assetPathDic.Clear();
        }

        protected override void OnInvalid()
        {
            throw new System.NotImplementedException();
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            AddLoadCount(path);
            return ResManager.Instance.Load<T>(path);
        }
        
        public void UnLoad(string path, int count = 1)
        {
            if (m_assetPathDic.ContainsKey(path))
            {
                ResManager.Instance.UnLoad(path, m_assetPathDic[path]);
            }
        }

        void AddLoadCount(string path)
        {
            if (!m_assetPathDic.ContainsKey(path))
            {
                m_assetPathDic.Add(path, 1);
            }
            else
            {
                m_assetPathDic[path] += 1;
            }
        }
    }
}