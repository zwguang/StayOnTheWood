using UnityEngine;

/// <summary>
    /// 所有继续自MonoBehaviour类的单例的基类，以免每个单都写一次
    /// 继承自MonoSingleton的好处是可以在场景上可以视化设置吧
    /// author Oscar
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance = null;
        private static Transform _transform;

        enum State
        {
            None,Create,AutoCreate,Destroy, AutoDestroy//  未创建过，,调用创建，自动创建, 手动删除，自动被清掉，
        }
        private static State _state = State.None; 

        /// <summary>
        ///  获取单例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (_state == State.AutoDestroy) //自动清掉，就不再创建，因为可能是运行结束自动清掉的
                    {
                        Debug.LogError("Mono单例已经自动被场景销毁，不可再创建！");
                        return null;
                    }
                    if(Application.isPlaying==false)
                    {
                        Debug.LogError("非运行时，不创建Mono单例！");
                        return null;
                    }

                    if (_transform == null)
                    {
                        _transform = GameObject.Find("GameRoot").transform; //ADKTool.GetADKRootChild("Singletons");

                    }
                    var type = typeof(T);
                    var instanceName = type.FullName;
                    var instanceTf = _transform.Find(instanceName);
                    if (instanceTf == null)
                    {
                        _state = State.Create;
                        var go = new GameObject(instanceName);
                        instanceTf = go.transform;
                        instanceTf.SetParent(_transform);
                    }

                    _instance = instanceTf.GetComponent<T>(); //FindObjectOfType<T>();
                    if (_instance == null)
                        _instance = instanceTf.gameObject.AddComponent<T>();

                }
                return _instance;
            }
        }

        public static bool HasInstance()
        {
             return _instance!=null;
        }
        public static void DestroyInstance()
        {
            if (HasInstance())
                _instance.Destroy();
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                if (Application.isPlaying)
                    Destroy(gameObject);
                else
                    DestroyImmediate(gameObject); 
            }
            if (_instance == null)
                _instance = this as T;
            if (_state != State.Create)
            {
                _state = State.AutoCreate;
            }
        }
        public  void Destroy()
        {
            _state = State.Destroy;
             GameObject.Destroy(this);
            _instance = null;
        }
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
                if(_state != State.Destroy)
                    _state = State.AutoDestroy;
            } 
        }


    }