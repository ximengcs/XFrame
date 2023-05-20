using System;

namespace XFrame.Modules.Pools
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PoolHelperAttribute : Attribute
    {
        public Type Target { get; }

        public PoolHelperAttribute(Type target)
        {
            Target = target;
        }
    }
}
 