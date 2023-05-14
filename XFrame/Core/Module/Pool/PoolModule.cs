using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    [BaseModule]
    public class PoolModule : SingletonModule<PoolModule>
    {
        private object[] m_ParamCache;
        private Dictionary<Type, IPool> m_PoolContainers;

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_ParamCache = new object[1];
            m_PoolContainers = new Dictionary<Type, IPool>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (IPool pool in m_PoolContainers.Values)
                pool.ClearObject();
            m_PoolContainers = null;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 创建或获取一个对象池
        /// </summary>
        /// <typeparam name="T">对象池持有类型</typeparam>
        /// <returns>对象池</returns>
        public IPool<T> GetOrNew<T>(IPoolHelper helper = null) where T : IPoolObject
        {
            return InnerGetOrNew(typeof(T), helper) as IPool<T>;
        }

        /// <summary>
        /// 创建或获取一个对象池
        /// </summary>
        /// <param name="objType">对象池持有数据类型</param>
        /// <returns>对象池</returns>
        public IPool GetOrNew(Type objType, IPoolHelper helper = null)
        {
            return InnerGetOrNew(objType, helper);
        }
        #endregion

        #region Inner Implement
        internal IPool InnerGetOrNew(Type objType, IPoolHelper helper = null)
        {
            if (!m_PoolContainers.TryGetValue(objType, out IPool pool))
            {
                if (helper == null)
                    helper = new DefaultPoolHelper();
                Type poolType = typeof(ObjectPool<>).MakeGenericType(objType);
                m_ParamCache[0] = helper;
                pool = Activator.CreateInstance(poolType, m_ParamCache) as IPool;
                m_PoolContainers.Add(objType, pool);
            }

            return pool;
        }
        #endregion
    }
}
