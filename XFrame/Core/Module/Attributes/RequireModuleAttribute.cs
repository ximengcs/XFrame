using System;

namespace XFrame.Core
{
    /// <summary>
    /// 依赖模块
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RequireModuleAttribute : Attribute
    {
        /// <summary>
        /// 依赖类型
        /// </summary>
        public Type ModuleType { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="type">依赖类型</param>
        public RequireModuleAttribute(Type type)
        {
            ModuleType = type;
        }
    }
}
