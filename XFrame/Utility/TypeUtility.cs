using System;
using System.Text;

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
        /// 取得简易名称
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>简易名</returns>
        public static string GetSimpleName(Type type)
        {
            string[] name = type.Name.Split('`');
            if (name.Length == 1)
            {
                return name[0];
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name[0]);
                sb.Append('<');

                Type[] generics = type.GenericTypeArguments;
                for (int i = 0; i < generics.Length; i++)
                {
                    sb.Append(generics[i].Name);
                    if (i < generics.Length - 1)
                        sb.Append(", ");
                }
                sb.Append('>');
                return sb.ToString();
            }
        }
    }
}
