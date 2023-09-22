
using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    public static class References
    {
        public static IPoolObject Require(Type type)
        {
            IPool pool = ModuleUtility.Pool.GetOrNew(type);
            return pool.Require();
        }

        public static T Require<T>() where T : IPoolObject
        {
            IPool<T> pool = ModuleUtility.Pool.GetOrNew<T>();
            return pool.Require();
        }

        public static void Release(IPoolObject obj)
        {
            IPool pool = ModuleUtility.Pool.GetOrNew(obj.GetType());
            pool.Release(obj);
        }
    }
}
