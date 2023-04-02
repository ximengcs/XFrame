using System;

namespace XFrame.Core
{
    /// <summary>
    /// 依赖模块
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class RequireModuleAttribute : Attribute
    {
        public Type ModuleType { get; }

        public RequireModuleAttribute(Type type)
        {
            ModuleType = type;
        }
    }
}
