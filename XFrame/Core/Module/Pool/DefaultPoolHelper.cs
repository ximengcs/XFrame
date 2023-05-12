﻿using System;

namespace XFrame.Modules.Pools
{
    internal class DefaultPoolHelper : IPoolHelper
    {
        IPoolObject IPoolHelper.Factory(Type type)
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
