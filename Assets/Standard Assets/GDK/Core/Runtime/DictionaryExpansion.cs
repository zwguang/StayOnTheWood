using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDK
{
    /// <summary>
    /// 对Dictionary类的拓展
    /// 1.静态类
    /// 2.静态方法
    /// 3.第一个参数指定方法所操作的类型，此参数前必须加this
    /// </summary>
    public static class DictionaryExpansion
    {
        /// <summary>
        /// 第一个参数代表要查找的字典
        /// </summary>
        public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value;
            dict.TryGetValue(key, out value);
            return value;
        }
    }
}