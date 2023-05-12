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
    }
}
