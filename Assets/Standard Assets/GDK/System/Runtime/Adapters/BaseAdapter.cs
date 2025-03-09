using System;

namespace GDK
{
    /// <summary>
    /// 为了适应不同的项目或引擎，把通用的接口接象出来，做成适配器。
    /// </summary>
    public abstract class BaseAdapter : IDisposable
    {
        public static T Create<T>() where T : BaseAdapter, new()
        {
            // T adapter=null;
            // if(PoolManager.HasInstance())
            // {
            //     adapter=PoolManager.Instance.GetObject<T>(false);
            // }
            // if (adapter == null)
            //     adapter = new T();

            //todo 优化从对象池获取
            T adapter = new T();
            return adapter;
        }

        protected bool isDisposed { get; private set; }

        /// <summary>
        /// 使它失效
        /// </summary>
        public void Invalid()
        {
            OnInvalid();
        }

        /// <summary>
        /// 销毁并释放
        /// </summary>
        public void Dispose()
        {
            isDisposed = true;
            OnDispose();
        }

        protected abstract void OnInvalid();


        protected abstract void OnDispose();
    }
}