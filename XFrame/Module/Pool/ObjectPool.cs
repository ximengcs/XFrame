using System;
using XFrame.Collections;

namespace XFrame.Modules
{
    internal class ObjectPool<T> : IPool<T> where T : IPoolObject
    {
        private XLoopQueue<IPoolObject> m_Objects;

        public int Capacity => m_Objects.Capacity;

        public ObjectPool(int capacity)
        {
            m_Objects = new XLoopQueue<IPoolObject>(capacity);
        }

        public bool Require(out T obj)
        {
            if (m_Objects.Empty)
            {
                obj = Activator.CreateInstance<T>();
                obj.OnCreate();
                return true;
            }
            else
            {
                obj = (T)m_Objects.RemoveFirst();
                obj.OnCreate();
                return false;
            }
        }

        public void Release(T obj)
        {
            if (m_Objects.Full)
            {
                obj.OnDestroyFrom();
                Log.Warning("XFrame", $"Object pool is full.release fail {typeof(T).Name}");
            }
            else
            {
                obj.OnRelease();
                m_Objects.AddLast(obj);
            }
        }
    }
}
