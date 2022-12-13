using XFrame.Collections;

namespace XFrame.Modules
{
    internal class PoolSystem<T> : IPoolSystem<T> where T : IPoolObject
    {
        private const int DEFAULT_SIZE = 8;
        private XLoopQueue<IPool<T>> m_PoolContainer;

        public PoolSystem(int capacity)
        {
            m_PoolContainer = new XLoopQueue<IPool<T>>(capacity);
        }

        public IPool<T> Require(int capacity)
        {
            if (capacity <= 0)
                capacity = DEFAULT_SIZE;

            IPool<T> pool;
            if (m_PoolContainer.Empty)
            {
                pool = new ObjectPool<T>(capacity);

            }
            else
                pool = m_PoolContainer.RemoveFirst();

            return pool;
        }

        public IPool<T> Require()
        {
            return Require(DEFAULT_SIZE);
        }

        public void Release(IPool<T> pool)
        {
            if (m_PoolContainer.Full)
            {
                Log.Warning("XFrame", $"PoolSystem Release failed, container is full. {typeof(T).Name}");
                return;
            }
            else
            {
                m_PoolContainer.AddLast(pool);
            }
        }
    }
}
