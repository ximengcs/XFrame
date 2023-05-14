﻿using System;

namespace XFrame.Modules.Pools
{
    public interface IPoolHelper
    {
        int CacheCount { get; }

        protected internal IPoolObject Factory(Type type);

        protected internal void OnObjectCreate(IPoolObject obj);

        protected internal void OnObjectRequest(IPoolObject obj);

        protected internal void OnObjectRelease(IPoolObject obj);

        protected internal void OnObjectDestroy(IPoolObject obj);
    }
}
