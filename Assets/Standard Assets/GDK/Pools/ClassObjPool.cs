using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using GDK;

namespace Assets.Scripts.Common
{
    public class IgnoreInPoolUse : Attribute
    {

    }

    /// <summary>
    /// 对象池接口，用于隔离泛型
    /// </summary>
    public interface IObjPoolCtrl
    {
        void Release(PooledClassObject obj);

        void ClearUnused();
        int total { get; }
        int frees { get; }
    }


    /// <summary>
    /// 可池化的基类
    /// </summary>
    public class PooledClassObject 
    {
        [NonSerialized]
        public bool bInPool = false;
        /// <summary>
        /// 对象SeqNum，仅跟踪Handle有效性，不同客户端的usingSeq可能是不一样的，不能涉及到逻辑
        /// </summary>
        [IgnoreInPoolUse][NonSerialized]
        internal UInt32 usingSeq = 0;
        //Pool分配器
        [IgnoreInPoolUse][NonSerialized]
        public IObjPoolCtrl holder;
        //是否要做字段复位检查
        [NonSerialized]
        public bool bChkReset = true;

        //对象从池中被启用
        public virtual void OnUse()
        {
        }
        //对象还回池中善后
        public virtual void OnRelease()
        {
        }

        public void Release()
        {

            if (holder != null)
            {
                OnRelease();
                holder.Release(this);
            }
        }
    }

    /// <summary>
    /// PooledClassObject对象引用包装器
    /// 为true可使用，禁止私自保存handle
    /// 主要为了防止外部直接持有PooledClassObject对象。因为对象被回收拿出，直接持有的话外部无法知道。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct PoolObjHandle<T> : IEquatable<PoolObjHandle<T>> where T : PooledClassObject
    {
        internal UInt32 _handleSeq;
        public T _handleObj;

        public PoolObjHandle(T obj)
        {
            if (obj != null && obj.usingSeq > 0)
            {
                _handleSeq = obj.usingSeq;
                _handleObj = obj;
            }
            else
            {
                _handleSeq = 0;
                _handleObj = null;
            }
        }

        public void Validate()
        {
            _handleSeq = _handleObj != null ? _handleObj.usingSeq : 0;
        }

        //释放引用
        public void Release()
        {
            _handleObj = null;
            _handleSeq = 0;
        }

        public void Set(T obj)
        {
            _handleObj = obj;
            _handleSeq = obj.usingSeq;
        }

        public void Set(ref PoolObjHandle<PooledClassObject> objHandle)
        {
            _handleObj = objHandle._handleObj as T;
            _handleSeq = objHandle._handleSeq;
        }

        //判断引用是否有效
        [System.Obsolete("operator bool is Obsolete ,use HasValue instead.")]
        public static implicit operator bool(PoolObjHandle<T> ptr)
        {
            return ptr._handleObj != null && ptr._handleObj.usingSeq == ptr._handleSeq;
        }
        public bool HasValue
        {
            get
            {
                return _handleObj != null && _handleObj.usingSeq == _handleSeq;
            }
        }
        [System.Obsolete("请不要和null做比较！！！！")]
        public static bool operator ==(PoolObjHandle<T> lhs, PoolObjHandle<T> rhs)
        {
            return lhs._handleObj == rhs._handleObj && lhs._handleSeq == rhs._handleSeq;
        }

        [System.Obsolete("请不要和null做比较！！！！")]
        public static bool operator !=(PoolObjHandle<T> lhs, PoolObjHandle<T> rhs)
        {
            return lhs._handleObj != rhs._handleObj || lhs._handleSeq != rhs._handleSeq;
        }

        public bool Equals(PoolObjHandle<T> other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj != null &&
                this.GetType() == obj.GetType() &&
                this == (PoolObjHandle<T>)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //自动转换成原始对象引用
        public static implicit operator T(PoolObjHandle<T> ptr)
        {
            return ptr.handle;
        }

        //获取对象引用
        public T handle
        {
            get
            {
#if UNITY_EDITOR 
                SDebug.Assert(_handleObj != null && _handleObj.usingSeq == _handleSeq);  
#endif
                return _handleObj;
            }
        }
    }

    public class ClassObjPoolRepository : Singleton<ClassObjPoolRepository>
    {
        public List<KeyValuePair<Type, IObjPoolCtrl>> Repositories = new List<KeyValuePair<Type, IObjPoolCtrl>>();

        public void Add(Type InType, IObjPoolCtrl InCtrl)
        {
            Repositories.Add(new KeyValuePair<Type, IObjPoolCtrl>(InType, InCtrl));
        }

        public void Clear()
        {
            for (int i = 0; i < Repositories.Count; ++i)
            {
                Repositories[i].Value.ClearUnused();
            }
        }
    }

    public abstract class ClassObjPoolBase : IObjPoolCtrl
    {
        /** Internal pool */
        protected List<object> pool = new List<object>(128);
        protected UInt32 reqSeq;

        protected UInt32 totals;

        public abstract void Release(PooledClassObject obj);

        public int capacity
        {
            get
            {
                return pool.Capacity;
            }
            set
            {
                pool.Capacity = value;
            }
        }

        public int total { get { return (int)totals; } }
        public abstract void ClearUnused();
        public abstract int frees { get; }
    }

    /// <summary>
    /// Pool对象分配器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClassObjPool<T> : ClassObjPoolBase where T : PooledClassObject, new()
    {
        private static ClassObjPool<T> instance = null;

#if UNITY_EDITOR && !SGAME_PROFILE_GC
        static T _default = new T();
#endif

        public static void NewSeq(T ojb)
        {
            if (instance == null)
            {
                instance = new ClassObjPool<T>();

                ClassObjPoolRepository.Instance.Add(typeof(T), instance);
            }

            instance.reqSeq++;
            ojb.usingSeq = instance.reqSeq;
        }

        public static T Get()
        {
            if (instance == null)
            {
                instance = new ClassObjPool<T>();

                ClassObjPoolRepository.Instance.Add(typeof(T), instance);
            }

            if (instance.pool.Count > 0)
            {
                T ls = (T)instance.pool[instance.pool.Count - 1];
                instance.pool.RemoveAt(instance.pool.Count - 1);
                ls.bInPool = false;
                instance.reqSeq++;
                ls.usingSeq = instance.reqSeq;
                ls.holder = instance;

                ls.OnUse();

#if UNITY_EDITOR && !SGAME_PROFILE_GC
                //编辑器下检查OnUse重置是否完整，如果从对象池中获得的对象存在脏数据可能是一个极大的不确定风险
                if (ls.bChkReset)
                {
                    System.Reflection.FieldInfo[] tar = ls.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    System.Reflection.FieldInfo[] ori = _default.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    UnityEngine.Debug.Assert(tar.Length == ori.Length, "对象池属性Reset字段不匹配");
                    for (int i = 0; i < ori.Length; i++)
                    {
                        var ignore = tar[i].GetCustomAttributes(typeof(IgnoreInPoolUse), true);
                        if (ignore != null && ignore.Length > 0)
                            continue;

                        if (tar[i].Name != ori[i].Name)
                        {
                            UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                        }
                        else
                        {
                            var ftar = tar[i].GetValue(ls);
                            var fori = ori[i].GetValue(_default);
                            if (ftar is ICollection)
                            {
                                if ((tar[i].FieldType != ori[i].FieldType))
                                {
                                    UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                                }
                                else if (tar[i].FieldType.IsArray)
                                {
                                    var atar = ftar as Array;
                                    var aori = fori as Array;
                                    if (atar.Length != aori.Length)
                                    {
                                        UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                                    }
                                    else
                                    {
                                        for (int e = 0; e < atar.Length; e++)
                                        {
                                            if (!System.Object.Equals(atar.GetValue(e), aori.GetValue(e)))
                                            {
                                                UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if ((ftar as ICollection).Count != 0 || (fori as ICollection).Count != 0)
                                    {
                                        UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                                    }
                                }
                            }
                            // else if (ftar is ListValueViewBase)
                            // {
                            //     if ((tar[i].FieldType != ori[i].FieldType))
                            //     {
                            //         UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                            //     }
                            //     else if ((ftar as ListValueViewBase).Count != 0 || (fori as ListValueViewBase).Count != 0)
                            //     {
                            //         UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                            //     }
                            // }
                            else if (tar[i].FieldType.FullName.Contains("CrypticInt32"))
                            {
                                int x = HackCrypticInt32Int(ftar);
                                int y = HackCrypticInt32Int(fori);
                                if (x != y)
                                {
                                    UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                                }
                            }
                            else if ((ftar != null && !ftar.Equals(fori)) || !System.Object.Equals(ftar, fori))
                            {
                                UnityEngine.Debug.LogError(string.Format("[{0}]属性[{1}]不一致", ls.GetType().Name, tar[i].Name));
                            }
                        }
                    }
                }
#endif
                return ls;
            }
            else
            {
                var ls = new T();

                instance.reqSeq++;
                ls.usingSeq = instance.reqSeq;
                ls.holder = instance;

                ls.OnUse();

                //UnityEngine.Debug.Log("[ClassObjPool New] " + typeof(T).ToString() + " Number: " + instance.totals);

                instance.totals++;

                return ls;
            }
        }

#if UNITY_EDITOR && !SGAME_PROFILE_GC
        static int HackCrypticInt32Int(object InValue)
        {
            var Methods = InValue.GetType().GetMethods();

            for (int i = 0; i < Methods.Length; ++i)
            {
                if (Methods[i].Name == "ToInt")
                {
                    int Value = Convert.ToInt32(Methods[i].Invoke(InValue, null));

                    return Value;
                }
            }

            return 0;
        }
#endif

        public override void Release(PooledClassObject obj)
        {

#if (DEBUG) && !SGAME_PROFILE_GC
            //var tobj = obj as T;
            //UnityEngine.Debug.Assert(tobj != null);
            //for (int i = 0; i < pool.Count; i++)
            //    if (pool[i] == obj)
            //        throw new System.InvalidOperationException("The object is released even though it is in the pool. Are you releasing it twice?");
            UnityEngine.Debug.Assert((obj as T) != null);
            if (obj.bInPool)
            {
                throw new System.InvalidOperationException("The object is released even though it is in the pool. Are you releasing it twice?");
            }
#endif
            if (obj.bInPool)
            {
                SDebug.LogWarning($"已经在缓存池了，还来添加!!!!!!  这东东叫{obj.GetType().Name}");
                return;
            }
            obj.usingSeq = 0;
            obj.holder = null;
            pool.Add(obj);
            obj.bInPool = true;
        }

        public override void ClearUnused()
        {
            totals -= (uint)pool.Count;
            pool.Clear();
        }

        public override int frees { get { return instance.pool.Count; } }

        public static void AdjustCaptain(int count)
        {
            if (instance == null)
            {
                instance = new ClassObjPool<T>();

                ClassObjPoolRepository.Instance.Add(typeof(T), instance);
            }

            count -= instance.pool.Count;

            for (int i = 0; i < count; ++i)
            {
                var ls = new T();

                ls.usingSeq = 0;
                ls.holder = null;

                ls.OnUse();

                instance.totals++;
                ls.bInPool = true;
                instance.pool.Add(ls);
            }
        }
    }


    // 通用的class对象池
    public sealed class SimpleClassPool<T> : IObjPoolCtrl where T : class
    {
        static SimpleClassPool<T> _instance;
        Stack<T> pool;
        Func<T> createAction;
        private int _total = 0;
        public int total  { get { return _total;} }

        public int frees { get { return pool.Count; } }

        private static SimpleClassPool<T> GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SimpleClassPool<T>();
                _instance.pool = new Stack<T>(4);
                ClassObjPoolRepository.Instance.Add(typeof(T), _instance);
            }

            return _instance;
        }

        public static T Get()
        {
            var pool = GetInstance().pool;
            var createAction = GetInstance().createAction;
            if (pool.Count > 0)
            {
                return pool.Pop();
            }
            GetInstance()._total++;
            if (createAction != null)
            {
                return createAction();
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T)) ;
            } 
        }

        // obj自己清理
        public static void Release(T obj)
        {
            var pool = GetInstance().pool;
            pool.Push(obj);
        }

        public void ClearUnused()
        {
            _total -= pool.Count;
            pool.Clear();
        }

        public static void AdjustCaptain(int count, Func<T> createAction)
        {
            var pool = GetInstance().pool;
            if (pool == null)
            {
                pool = new Stack<T>(count);
                GetInstance().pool = pool;
            }
            GetInstance().createAction = createAction;
            count -= pool.Count;

            for (int i = 0; i < count; ++i)
            {
                var ls = createAction();
                GetInstance()._total++;
                pool.Push(ls);
            }
        }

        // makes compile happy
        public void Release(PooledClassObject obj)
        {
            throw new NotImplementedException();
        }
    }

}
