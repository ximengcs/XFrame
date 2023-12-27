using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : PoolHelperBase
    {
        protected internal override IPoolObject Factory(Type type, int poolKey = 0, object userData = null)
        {
            return (IPoolObject)XModule.Type.CreateInstance(type);
        }
    }
}
