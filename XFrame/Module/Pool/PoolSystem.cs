using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using XFrame.Modules;

namespace XFrame.Modules
{
    public class PoolSystem : IPoolSystem
    {
        private int m_Capacity;
        private Dictionary<Type, IPool> m_Pools;

        public PoolSystem(Type type, int capacity)
        {
            RootType = type;
            m_Capacity = capacity;
            m_Pools = new Dictionary<Type, IPool>();
        }

        public Type RootType { get; }

        public IPool Get<T>() where T : IPoolObject
        {
            return Get(typeof(T));
        }

        public IPool Get(Type type)
        {
            if (!m_Pools.TryGetValue(type, out IPool pool))
            {
                pool = new ObjectPool(type, m_Capacity);
                m_Pools.Add(type, pool);
            }

            return pool;
        }

        public IEnumerator<IPool> GetEnumerator()
        {
            return m_Pools.Values.GetEnumerator();
        }

        public void Remove(IPool pool)
        {
            m_Pools.Remove(pool.GetType());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
