using System;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : IPoolHelper
    {
        public IPoolObject Factory(Type type)
        {
            return (IPoolObject)Activator.CreateInstance(type);
        }
    }
}
