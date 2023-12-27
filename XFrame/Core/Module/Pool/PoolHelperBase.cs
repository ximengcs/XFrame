
using System;

namespace XFrame.Modules.Pools
{
    internal abstract class PoolHelperBase : IPoolHelper
    {
        public int CacheCount { get; protected set; } = 64;

        protected internal abstract PoolObjectBase Factory(Type type, int poolKey = default, object userData = default);

        protected internal virtual void OnObjectCreate(PoolObjectBase obj) { }

        protected internal virtual void OnObjectRequest(PoolObjectBase obj) { }

        protected internal virtual void OnObjectRelease(PoolObjectBase obj) { }

        protected internal virtual void OnObjectDestroy(PoolObjectBase obj) { }
    }
}
