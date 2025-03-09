using System;
using System.Collections.Generic;

namespace GDK
{
    public class ResAdapter : BaseAdapter
    {
        Dictionary<string, int> m_assetPathDic = new Dictionary<string, int>();

        private SystemResManager handler = SystemResManager.Instance;

        protected override void OnDispose()
        {
            foreach (var v in m_assetPathDic)
            {
                handler.UnLoad(v.Key, v.Value);
            }

            m_assetPathDic.Clear();
        }

        protected override void OnInvalid()
        {
            // throw new System.NotImplementedException();
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Load(path, typeof(T)) as T;
        }

        public object Load(string path, Type type = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                SDebug.Log("ResAdapter.Load 路径不能为空");
                return null;
            }

            object asset;
            if (HasRes(path, type, out asset))
            {
                return asset;
            }
            else
            {
                AddLoadCount(path);
                asset = handler.Load(path, type);
                return asset;
            }
        }

        public void UnLoad(string path)
        {
            if (m_assetPathDic.ContainsKey(path))
            {
                handler.UnLoad(path, m_assetPathDic[path]);
            }
        }

        private bool HasRes(string path, Type type, out object asset)
        {
            if (m_assetPathDic == null || m_assetPathDic.ContainsKey(path) == false)
            {
                asset = null;
                return false;
            }
            else
            {
                asset = handler.GetRes(path, type);
                if (asset != null || handler.IsDone(path))
                    return true;
                return false;
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