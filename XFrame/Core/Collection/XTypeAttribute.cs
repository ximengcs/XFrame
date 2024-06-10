using System;

namespace XFrame.Collections
{
    /// <summary>
    /// 映射到类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class XTypeAttribute : Attribute
    {
        /// <summary>
        /// 目标类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="type">目标类型</param>
        public XTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
