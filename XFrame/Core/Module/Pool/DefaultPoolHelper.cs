using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : PoolHelperBase
    {
        protected internal override IXPoolObject Factory(Type type, int poolKey = 0, object userData = null)
        {
            return (IXPoolObject)XModule.Type.CreateInstance(type);
        }
    }
}
