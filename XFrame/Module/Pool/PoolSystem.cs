using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Pools
{
    internal class PoolSystem<T> : IPoolSystem<T> where T : IPoolObject
    {
        private const int DEFAULT_SIZE = 8;
        private Dictionary<Type, XLoopQueue<IPool>> m_PoolContainers;

        public PoolSystem(int capacity)
        {
            m_PoolContainers = new Dictionary<Type, XLoopQueue<IPool>>(capacity);
        }

        public IPool Require(Type poolObjType, int capacity)
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;

            if (!m_PoolContainers.TryGetValue(poolObjType, out XLoopQueue<IPool> poolQueue))
            {
                poolQueue = new XLoopQueue<IPool>(capacity);
                m_PoolContainers[poolObjType] = poolQueue;
            }

            IPool pool;
            if (poolQueue.Empty)
            {
                Type poolType = typeof(ObjectPool<>).MakeGenericType(poolObjType);
                pool = Activator.CreateInstance(poolType, capacity) as IPool;
            }
            else
                pool = poolQueue.RemoveFirst();

            return pool;
        }

        public IPool<PoolType> Require<PoolType>(int capacity) where PoolType : T
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;

            Type type = typeof(PoolType);
            if (!m_PoolContainers.TryGetValue(type, out XLoopQueue<IPool> poolQueue))
            {
                poolQueue = new XLoopQueue<IPool>(capacity);
                m_PoolContainers[type] = poolQueue;
            }

            IPool<PoolType> pool;
            if (poolQueue.Empty)
                pool = new ObjectPool<PoolType>(capacity);
            else
                pool = poolQueue.RemoveFirst() as IPool<PoolType>;

            return pool;
        }

        public IPool Require(Type poolObjType)
        {
            return Require(poolObjType, DEFAULT_SIZE);
        }

        public IPool<PoolType> Require<PoolType>() where PoolType : T
        {
            return Require<PoolType>(DEFAULT_SIZE);
        }

        public void Release(IPool pool)
        {
            Type type = pool.ObjectType;
            if (!m_PoolContainers.TryGetValue(type, out XLoopQueue<IPool> poolQueue))
            {
                poolQueue = new XLoopQueue<IPool>(DEFAULT_SIZE);
                m_PoolContainers[type] = poolQueue;
            }

            if (poolQueue.Full)
            {
                Log.Warning("XFrame", $"PoolSystem Release failed, container is full. {typeof(T).Name}");
                return;
            }
            else
            {
                poolQueue.AddLast(pool);
            }
        }
    }
}
