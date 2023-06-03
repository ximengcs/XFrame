
using System;

namespace XFrame.Modules.Pools
{
    public static class References
    {
        public static IPoolObject Require(Type type)
        {
            IPool pool = PoolModule.Inst.GetOrNew(type);
            return pool.Require();
        }

        public static T Require<T>() where T : IPoolObject
        {
            IPool<T> pool = PoolModule.Inst.GetOrNew<T>();
            return pool.Require();
        }

        public static void Release(IPoolObject obj)
        {
            IPool pool = PoolModule.Inst.GetOrNew(obj.GetType());
            pool.Release(obj);
        }
    }
}
