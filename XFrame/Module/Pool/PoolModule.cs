using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules
{
    public class PoolModule : SingleModule<PoolModule>
    {
        private const int DEFAULT_SIZE = 8;
        private Dictionary<Type, IPoolSystem> m_Objects;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Objects = new Dictionary<Type, IPoolSystem>();
        }

        public IPoolSystem Register<T>(int capacity) where T : IPoolObject
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;
            IPoolSystem pool = InnerGetOrNewPool<T>(capacity);
            return pool;
        }

        public IPoolSystem Get<T>() where T : IPoolObject
        {
            return InnerGetPool(typeof(T));
        }

        private IPoolSystem InnerGetPool(Type type)
        {
            if (m_Objects.TryGetValue(type, out IPoolSystem pool))
                return pool;
            else
                return default;
        }

        private IPoolSystem InnerGetOrNewPool<T>(int capacity) where T : IPoolObject
        {
            Type type = typeof(T);
            IPoolSystem pool;
            if (!m_Objects.TryGetValue(type, out pool))
            {
                pool = new PoolSystem(type, capacity);
                m_Objects.Add(type, pool);
            }

            return pool;
        }
    }
}
