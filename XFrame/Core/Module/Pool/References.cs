
using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    public static class References
    {
        private static IPoolModule s_Module;
        public static bool Available => s_Module != null;

        public static void SetDomain(XDomain domain)
        {
            s_Module = domain.GetModule<IPoolModule>();
        }

        public static IPoolObject Require(Type type)
        {
            IPool pool = s_Module.GetOrNew(type);
            return pool.Require();
        }

        public static T Require<T>() where T : IPoolObject
        {
            IPool<T> pool = s_Module.GetOrNew<T>();
            return pool.Require();
        }

        public static void Release(IPoolObject obj)
        {
            IPool pool = s_Module.GetOrNew(obj.GetType());
            pool.Release(obj);
        }
    }
}
