using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Pools
{
    internal class ObjectPool<T> : IPool<T> where T : IPoolObject
    {
        private Type m_Type;
        private XLoopQueue<IPoolObject> m_Objects;

        public int Capacity => m_Objects.Capacity;

        public Type ObjectType => m_Type;

        public ObjectPool(int capacity)
        {
            m_Type = typeof(T);
            m_Objects = new XLoopQueue<IPoolObject>(capacity);
        }

        public bool Require(out T obj)
        {
            bool isNew = InnerRequire(out IPoolObject target);
            obj = (T)target;
            return isNew;
        }

        public void Release(T obj)
        {
            InnerRelease(obj);
        }

        public bool Require(out IPoolObject obj)
        {
            return InnerRequire(out obj);
        }

        public void Release(IPoolObject obj)
        {
            InnerRelease(obj);
        }

        private bool InnerRequire(out IPoolObject obj)
        {
            if (m_Objects.Empty)
            {
                obj = Activator.CreateInstance(m_Type) as IPoolObject;
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

        private void InnerRelease(IPoolObject obj)
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
