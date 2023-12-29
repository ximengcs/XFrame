
using System;

namespace XFrame.Modules.Pools
{
    internal abstract class PoolHelperBase : IPoolHelper
    {
        public int CacheCount { get; protected set; } = 64;

        protected internal abstract IXPoolObject Factory(Type type, int poolKey = default, object userData = default);

        protected internal virtual void OnObjectCreate(IPoolObject obj) { }

        protected internal virtual void OnObjectRequest(IPoolObject obj) { }

        protected internal virtual void OnObjectRelease(IPoolObject obj) { }

        protected internal virtual void OnObjectDestroy(IPoolObject obj) { }
    }
}
