﻿using System;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : IPoolHelper
    {
        public int CacheCount => 64;

        IPoolObject IPoolHelper.Factory(Type type, int poolKey, object userData)
        {
            return (IPoolObject)Activator.CreateInstance(type);
        }

        void IPoolHelper.OnObjectCreate(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectDestroy(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRelease(IPoolObject obj)
        {

        }

        void IPoolHelper.OnObjectRequest(IPoolObject obj)
        {

        }
    }
}
