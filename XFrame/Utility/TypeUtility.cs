using System;

namespace XFrame.Utility
{
    /// <summary>
    /// 类型常用方法
    /// </summary>
    public class TypeUtility
    {
        /// <summary>
        /// 取得简易名称
        /// </summary>
        /// <param name="fullName">类型全名</param>
        /// <returns>简易名</returns>
        public static string GetSimpleName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;
            string[] names = fullName.Split('.');
            return names[names.Length - 1];
        }

        /// <summary>
        /// 是否存在特性
        /// </summary>
        /// <param name="inst">源对象实例</param>
        /// <param name="type">特性类型</param>
        /// <returns>true为存在</returns>
        public static bool HasAttribute(object inst, Type type)
        {
            return HasAttribute(inst.GetType(), type);
        }

        /// <summary>
        /// 是否存在特性
        /// </summary>
        /// <param name="instType">源对象类型</param>
        /// <param name="type">特性类型</param>
        /// <returns>true为存在</returns>
        public static bool HasAttribute(Type instType, Type type)
        {
            return Attribute.GetCustomAttribute(instType, type, true) != null;
        }

        /// <summary>
        /// 是否存在特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="inst">源对象实例</param>
        /// <returns>true存在</returns>
        public static bool HasAttribute<T>(object inst) where T : Attribute
        {
            return HasAttribute(inst.GetType(), typeof(T));
        }

        /// <summary>
        /// 是否存在特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="type">源对象类型</param>
        /// <returns>true为存在</returns>
        public static bool HasAttribute<T>(Type type) where T : Attribute
        {
            return HasAttribute(type, typeof(T));
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="inst">源对象实例</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(object inst) where T : Attribute
        {
            return Attribute.GetCustomAttribute(inst.GetType(), typeof(T), true) as T;
        }

        public static Attribute GetAttribute(object inst, Type type)
        {
            return Attribute.GetCustomAttribute(inst.GetType(), type, true);
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T">特性类型</typeparam>
        /// <param name="type">源对象类型</param>
        /// <returns>特性</returns>
        public static T GetAttribute<T>(Type type) where T : Attribute
        {
            return Attribute.GetCustomAttribute(type, typeof(T), true) as T;
        }

        public static Attribute GetAttribute(Type type, Type attrType)
        {
            return Attribute.GetCustomAttribute(type, attrType, true);
        }
    }
}
