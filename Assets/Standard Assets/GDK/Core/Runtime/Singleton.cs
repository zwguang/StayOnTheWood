namespace GDK
{
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        static T m_instance = null;
        internal static int singletonCount = 0;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    singletonCount++;
                    m_instance = new T();
                    m_instance.OnConstruct();
                }

                return m_instance;
            }
        }
        
        protected Singleton(){}

        public static bool HasInstance()
        {
            return m_instance != null;
        }

        public static void DestroyInstance()
        {
            if (HasInstance())
            {
                m_instance.Destroy();
            }
        }
        
        protected virtual void OnConstruct()
        {
            
        }

        protected virtual void OnDestroy()
        {
            
        }
        
        protected virtual void Init(){}

        private void Destroy()
        {
            m_instance = null;
            singletonCount--;
            OnDestroy();
        }
    }
}