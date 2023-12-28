using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : PoolHelperBase
    {
        protected internal override PoolObjectBase Factory(Type type, int poolKey = 0, object userData = null)
        {
            return (PoolObjectBase)XModule.Type.CreateInstance(type);
        }
    }
}
