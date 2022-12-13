using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    public class PoolModule : SingleModule<PoolModule>
    {
        private const int DEFAULT_SIZE = 8;
        private Dictionary<Type, object> m_Objects;

        #region Life Fun
        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Objects = new Dictionary<Type, object>();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 创建或获取一个给定容量的对象池系统
        /// </summary>
        /// <typeparam name="T">对象池持有类型</typeparam>
        /// <param name="capacity">对象池系统容量</param>
        /// <returns>获取到的对象池系统</returns>
        public IPoolSystem<T> GetOrNew<T>(int capacity) where T : IPoolObject
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;
            return InnerGetOrNewPool<T>(capacity);
        }

        /// <summary>
        /// 创建或获取一个默认容量的对象池系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPoolSystem<T> GetOrNew<T>() where T : IPoolObject
        {
            return InnerGetOrNewPool<T>(DEFAULT_SIZE);
        }

        /// <summary>
        /// 创建或获取一个给定容量的对象池系统
        /// </summary>
        /// <typeparam name="T">对象池持有类型</typeparam>
        /// <param name="capacity">对象池系统容量</param>
        /// <returns>获取到的对象池系统</returns>
        public IPoolSystem<T> GetOrNew<T>(Type type, int capacity) where T : IPoolObject
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;
            return InnerGetOrNewPool<T>(type, capacity);
        }

        /// <summary>
        /// 创建或获取一个默认容量的对象池系统
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IPoolSystem<T> GetOrNew<T>(Type type) where T : IPoolObject
        {
            return InnerGetOrNewPool<T>(type, DEFAULT_SIZE);
        }
        #endregion

        #region Inner Implement
        private IPoolSystem<T> InnerGetOrNewPool<T>(int capacity) where T : IPoolObject
        {
            Type type = typeof(T);
            IPoolSystem<T> pool;
            if (!m_Objects.TryGetValue(type, out object cachePool))
            {
                pool = new PoolSystem<T>(capacity);
                m_Objects.Add(type, pool);
            }
            else
            {
                pool = cachePool as IPoolSystem<T>;
            }

            return pool;
        }

        private IPoolSystem<T> InnerGetOrNewPool<T>(Type type, int capacity) where T : IPoolObject
        {
            IPoolSystem<T> pool;
            if (!m_Objects.TryGetValue(type, out object cachePool))
            {
                pool = new PoolSystem<T>(capacity);
                m_Objects.Add(type, pool);
            }
            else
            {
                pool = cachePool as IPoolSystem<T>;
            }

            return pool;
        }
        #endregion
    }
}
