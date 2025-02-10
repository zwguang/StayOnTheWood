using System;
using UnityEngine;
using Object = System.Object;

namespace GDK
{
    public enum ResloaderStatus
    {
        None,
        Waiting,
        Loading,
        Done
    }
    
    public abstract class BaseResLoader
    {
        public string path;
        public Action<BaseResLoader> onLoad;
        public ResloaderStatus Status;
        public int referenceCount = 0;
        public Type type;
        public bool bReadable = false;

        protected bool isDispose = false;

        public object asset
        {
            get;
            set;
        }

        public abstract bool IsExits();
        public abstract object Load();
        public abstract void LoadAsync();

        public virtual void Dispose()
        {
            if (asset != null && (asset is UnityEngine.Texture || asset is UnityEngine.Sprite))
            {
                Resources.UnloadAsset(asset as UnityEngine.Object);
            }

            isDispose = true;
            onLoad = null;
            asset = null;
            referenceCount = 0;
        }

        public void Done()
        {
            if (Status == ResloaderStatus.Done)
            {
                return;
            }

            if (isDispose)
            {
                Dispose();
            }
            else
            {
                if (ReferenceEquals(asset, null))
                {
                    Debug.LogError($"加载资源失败, asset is null path = {path}");
                }

                Status = ResloaderStatus.Done;

                if (onLoad != null)
                {
                    onLoad(this);
                }
            }
        }
    }
    
}