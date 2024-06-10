
using System;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池引用
    /// </summary>
    public static class References
    {
        private static IPoolModule s_Module;
        
        /// <summary>
        /// 对象池是否准备好
        /// </summary>
        public static bool Available => s_Module != null;

        /// <summary>
        /// 设置域
        /// </summary>
        /// <param name="domain">域</param>
        public static void SetDomain(XDomain domain)
        {
            s_Module = domain.GetModule<IPoolModule>();
        }

        /// <summary>
        /// 请求一个对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <returns>对象实例</returns>
        public static IPoolObject Require(Type type)
        {
            IPool pool = s_Module.GetOrNew(type);
            return pool.Require();
        }

        /// <summary>
        /// 请求一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <returns>对象实例</returns>
        public static T Require<T>() where T : IPoolObject
        {
            IPool<T> pool = s_Module.GetOrNew<T>();
            return pool.Require();
        }

        /// <summary>
        /// 释放一个对象
        /// </summary>
        /// <param name="obj">目标对象</param>
        public static void Release(IPoolObject obj)
        {
            IPool pool = s_Module.GetOrNew(obj.GetType());
            pool.Release(obj);
        }
    }
}
