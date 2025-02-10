using System.Collections.Generic;

namespace GDK
{
    public static class ListExpand
    {
        /// <summary>
        /// 返回末尾的元素，并从列表移除
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Pop<T>(this List<T> list)
        {
            if (list.Count == 0)
            {
                return default(T); //对于引用类型，默认值将为null，而对于值类型，将为0（对于数字类型）或false（对于布尔类型）。
            }
            T item = list[list.Count - 1];
            list.Remove(item);
            return item;
        }
        
        /// <summary>
        /// 返回末尾的元素，不从列表移除
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Peek<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
    }
}