
using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    public interface IPoolModule : IModule
    {
        IEnumerable<IPool> AllPool { get; }

        IPool<T> GetOrNew<T>(IPoolHelper helper = null) where T : IPoolObject;

        IPool GetOrNew(Type objType, IPoolHelper helper = null);
    }
}
