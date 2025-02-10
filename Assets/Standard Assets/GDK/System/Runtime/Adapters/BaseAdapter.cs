using System;

namespace GDK
{
    public abstract class BaseAdapter : IDisposable
    {
        
        
        protected bool isDisposed
        {
            get;
            private set;
        }
        
        public void Invalid()
        {
            OnInvalid();
        }
        
        public void Dispose()
        {
            OnDispose();
        }
        
        protected abstract void OnInvalid();


        protected abstract void OnDispose();
    }
}