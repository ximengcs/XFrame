using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using System.Collections.Generic;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    [BaseModule]
    [XType(typeof(IPoolModule))]
    public class PoolModule : ModuleBase, IPoolModule
    {
        private object[] m_ParamCache;
        private Dictionary<Type, IPool> m_PoolContainers;

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_ParamCache = new object[2];
            m_PoolContainers = new Dictionary<Type, IPool>();

            Type helperType = typeof(IPoolHelper);
            TypeSystem typeSys = Domain.TypeModule.GetOrNewWithAttr<PoolHelperAttribute>();
            foreach (Type type in typeSys)
            {
                if (helperType.IsAssignableFrom(type))
                {
                    PoolHelperAttribute attr = Domain.TypeModule.GetAttribute<PoolHelperAttribute>(type);
                    IPoolHelper helper = Domain.TypeModule.CreateInstance(type) as IPoolHelper;
                    InnerGetOrNew(attr.Target, helper);
                }
            }
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
        /// 获取所有对象池的集合
        /// </summary>
        public IEnumerable<IPool> AllPool => m_PoolContainers.Values;

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
                    helper = new DefaultPoolHelper(this);
                Type poolType = typeof(ObjectPool<>).MakeGenericType(objType);
                m_ParamCache[0] = this;
                m_ParamCache[1] = helper;
                pool = Domain.TypeModule.CreateInstance(poolType, m_ParamCache) as IPool;
                m_PoolContainers.Add(objType, pool);
            }

            return pool;
        }
        #endregion
    }
}
