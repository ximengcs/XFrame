using System;

namespace XFrame.Modules.Reflection
{
    /// <summary>
    /// 可自动被类型系统添加
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class XAttribute : Attribute
    {
        /// <summary>
        /// 类型key
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="id">类型key</param>
        public XAttribute(int id)
        {
            Id = id;
        }
    }
}
