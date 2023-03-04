using System;

namespace XFrame.Core
{
    public class RequireModuleAttribute : Attribute
    {
        public Type ModuleType { get; }

        public RequireModuleAttribute(Type type)
        {
            ModuleType = type;
        }
    }
}
