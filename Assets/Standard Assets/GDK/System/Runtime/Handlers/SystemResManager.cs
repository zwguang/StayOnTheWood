using System;
using System.Collections.Generic;

namespace GDK
{
    public class SystemResManager : Singleton<SystemResManager>
    {
        public static bool AB_MODE = false;

        Dictionary<string, BaseResLoader> m_loaderDic = new Dictionary<string, BaseResLoader>();
        Dictionary<string, BaseResLoader> m_waitingDic = new Dictionary<string, BaseResLoader>();


        public bool IsExits(string path)
        {
            var loader = GetLoader(path);
            return loader.IsExits();
        }

        /// <summary>
        /// 此资源是否已经加载
        /// </summary>
        public bool IsDone(string path)
        {
            if (string.IsNullOrEmpty(path))
                return true;
            var loader = GetLoader(path, false);
            if (loader == null)
                return false;
            return loader.Status == ResloaderStatus.Done;
        }

        /// <summary>
        /// 获取已加载的资源，如果未加载，则为空
        /// </summary>
        public T GetRes<T>(string path) where T : UnityEngine.Object
        {
            return GetRes(path, typeof(T)) as T;
        }

        public object GetRes(string path, Type type = null)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            var loader = GetLoader(path, false);
            return loader?.asset;
        }

        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Load(path, typeof(T)) as T;
        }

        public object Load(string path, Type type = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var loader = GetLoader(path, true);
            loader.type = type ?? typeof(UnityEngine.Object);
            loader.path = path;

            if (loader.Status != ResloaderStatus.Done)
            {
                loader.Load();
            }

            loader.referenceCount++;
            return loader.asset;
        }

        public void UnLoad(string path, int count = 1)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            BaseResLoader loader = GetLoader(path);
            if (loader == null)
            {
                return;
            }

            if (count > 0)
            {
                loader.referenceCount -= count;
                if (loader.referenceCount <= 0)
                {
                    loader.referenceCount = 0;
                    Dispose(loader); //todo, 不能立即释放
                }
            }
        }

        private BaseResLoader GetLoader(string path, bool autoCreate = true)
        {
            BaseResLoader loader;
            if (m_loaderDic.TryGetValue(path, out loader))
            {
                return loader;
            }

            if (autoCreate)
            {
                //todo
                loader = new ResourcesResLoader();
                loader.path = path;
                loader.onLoad = OnLoad;
            }

            return loader;
        }

        /// <summary>
        /// 资源加载完成的回调，异步会用到
        /// </summary>
        /// <param name="loader"></param>
        private void OnLoad(BaseResLoader loader)
        {
            if (m_waitingDic.ContainsValue(loader))
            {
                m_waitingDic.Remove(loader.path);
            }
        }

        void Dispose(BaseResLoader loader)
        {
            if (m_waitingDic.ContainsKey(loader.path))
            {
                m_waitingDic.Remove(loader.path);
            }

            if (m_loaderDic.ContainsKey(loader.path))
            {
                m_loaderDic.Remove(loader.path);
            }

            loader.Dispose();
        }
    }
}