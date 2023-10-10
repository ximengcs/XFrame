
using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    public static class References
    {
        public static bool Available => XModule.Pool != null;

        public static IPoolObject Require(Type type)
        {
            IPool pool = XModule.Pool.GetOrNew(type);
            return pool.Require();
        }

        public static T Require<T>() where T : IPoolObject
        {
            IPool<T> pool = XModule.Pool.GetOrNew<T>();
            return pool.Require();
        }

        public static void Release(IPoolObject obj)
        {
            IPool pool = XModule.Pool.GetOrNew(obj.GetType());
            pool.Release(obj);
        }
    }
}
