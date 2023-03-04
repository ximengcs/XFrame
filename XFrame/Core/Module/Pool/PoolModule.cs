using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    [CoreModule]
    public class PoolModule : SingletonModule<PoolModule>
    {
        private Dictionary<Type, IPool> m_PoolContainers;

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_PoolContainers = new Dictionary<Type, IPool>();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 创建或获取一个给定容量的对象池系统
        /// </summary>
        /// <typeparam name="T">对象池持有类型</typeparam>
        /// <param name="capacity">对象池系统容量</param>
        /// <returns>获取到的对象池系统</returns>
        public IPool<T> GetOrNew<T>() where T : IPoolObject
        {
            return InnerGetOrNew(typeof(T)) as IPool<T>;
        }

        public IPool GetOrNew(Type objType)
        {
            return InnerGetOrNew(objType);
        }
        #endregion

        #region Inner Implement
        internal IPool InnerGetOrNew(Type objType)
        {
            if (!m_PoolContainers.TryGetValue(objType, out IPool pool))
            {
                Type poolType = typeof(ObjectPool<>).MakeGenericType(objType);
                pool = Activator.CreateInstance(poolType) as IPool;
                m_PoolContainers.Add(objType, pool);
            }

            return pool;
        }
        #endregion
    }
}
