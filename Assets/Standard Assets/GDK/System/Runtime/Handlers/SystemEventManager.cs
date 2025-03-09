using System;
using System.Collections.Generic;
using UnityEngine;

namespace GDK
{
    public class SystemEventManager : Singleton<SystemEventManager>
    {
        Dictionary<int, List<Delegate>> m_eventDic = new Dictionary<int, List<Delegate>>();

        #region 注册事件

        public void On(int type, Action callBack)
        {
            On(type, callBack as Delegate);
        }

        public void On<T>(int type, Action<T> callBack)
        {
            On(type, callBack as Delegate);
        }

        public void On<T1, T2>(int type, Action<T1, T2> callBack)
        {
            On(type, callBack as Delegate);
        }

        public void On<T1, T2, T3>(int type, Action<T1, T2, T3> callBack)
        {
            On(type, callBack as Delegate);
        }

        public void On(int type, Delegate callBack)
        {
            if (!m_eventDic.ContainsKey(type))
            {
                List<Delegate> d = new List<Delegate>();
                m_eventDic.Add(type, d);
            }

            var list = m_eventDic.GetValue(type);
            if (!list.Contains(callBack)) //todo  性能待优化
            {
                list.Add(callBack);
            }
            else
            {
                Debug.LogError(
                    $"相同事件重复添加 type:{type.ToString()}, callBack:{callBack?.Method.Name}, 委托类型：{callBack?.ToString()}");
            }
        }

        #endregion

        #region 派发事件

        public void Trigger(int type)
        {
            Trigger<object, object, object>(type, 0, null, null, null);
        }

        public void Trigger<T>(int type, T arg)
        {
            Trigger<T, object, object>(type, 1, arg, null, null);
        }

        public void Trigger<T1, T2>(int type, T1 arg1, T2 arg2)
        {
            Trigger<T1, T2, object>(type, 2, arg1, arg2, null);
        }

        public void Trigger<T1, T2, T3>(int type, T1 arg1, T2 arg2, T3 arg3)
        {
            Trigger<T1, T2, T3>(type, 3, arg1, arg2, arg3);
        }

        public void Trigger<T1, T2, T3>(int type, int argNum, T1 arg1, T2 arg2, T3 arg3)
        {
            var list = m_eventDic.GetValue(type);
            if (list != null)
            {
                var count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    var fun = list[i];
                    if (argNum == 0)
                    {
                        ((Action)fun)();
                    }
                    else if (argNum == 1)
                    {
                        if (fun is Action<T1>)
                        {
                            ((Action<T1>)fun)(arg1);
                        }
                    }
                    else if (argNum == 2)
                    {
                        if (fun is Action<T1, T2>)
                        {
                            ((Action<T1, T2>)fun)(arg1, arg2);
                        }
                    }
                    else if (argNum == 3)
                    {
                        if (fun is Action<T1, T2, T3>)
                        {
                            ((Action<T1, T2, T3>)fun)(arg1, arg2, arg3);
                        }
                    }
                }
            }
        }

        #endregion

        public void Off(int type, Action callback)
        {
            Off(type, callback as Delegate);
        }

        public void Off<T>(int type, Action<T> callback)
        {
            Off(type, callback as Delegate);
        }

        public void Off<T1, T2>(int type, Action<T1, T2> callback)
        {
            Off(type, callback as Delegate);
        }

        public void Off<T1, T2, T3>(int type, Action<T1, T2, T3> callback)
        {
            Off(type, callback as Delegate);
        }

        public void Off(int type, Delegate callback)
        {
            var list = m_eventDic.GetValue(type);

            if (list != null)
            {
                var index = list.IndexOf(callback);
                if (index != -1)
                {
                    list.RemoveAt(index);
                }
            }
            else
            {
                Debug.LogError($"事件类型不存在 type = {type.ToString()}");
            }
        }

        //todo 效率太低
        public void OffAll(object subscriber)
        {
            foreach (var values in m_eventDic.Values)
            {
                values.RemoveAll(d => d.Target == subscriber);
            }
        }
    }
}