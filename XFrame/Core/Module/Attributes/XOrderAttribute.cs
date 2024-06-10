using System;

namespace XFrame.Core
{
    /// <summary>
    /// 定义顺序
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class XOrderAttribute : Attribute
    {
        /// <summary>
        /// 顺序
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="order">顺序</param>
        public XOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
