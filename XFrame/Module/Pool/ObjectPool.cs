using System;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public class ObjectPool : IPool
    {
        private Type m_Type;
        private int m_Capacity;
        private IPoolObject[] m_Objects;
        private int m_L;
        private int m_R;

        private bool m_Empty => m_L == m_R;
        private bool m_Full => (m_R + 1) % m_Capacity == m_L;

        public ObjectPool(Type type, int capacity)
        {
            m_Type = type;
            m_Capacity = capacity;
            m_Objects = new IPoolObject[capacity];
            m_L = 0;
            m_R = 0;
        }

        public bool Require<T>(out T obj) where T : IPoolObject
        {
            if (m_Empty)
            {
                obj = Activator.CreateInstance<T>();
                obj.OnCreate(this);
                return true;
            }
            else
            {
                obj = (T)m_Objects[m_L];
                m_Objects[m_L] = default;
                m_L = (m_L + 1) % m_Capacity;
                obj.OnCreate(this);
                return false;
            }
        }

        public bool Require(Type type, out IPoolObject obj)
        {
            if (m_Empty)
            {
                obj = Activator.CreateInstance(type) as IPoolObject;
                obj.OnCreate(this);
                return true;
            }
            else
            {
                obj = m_Objects[m_L];
                m_Objects[m_L] = default;
                m_L = (m_L + 1) % m_Capacity;
                obj.OnCreate(this);
                return false;
            }
        }

        public void Release(IPoolObject obj)
        {
            if (obj.GetType() != m_Type)
            {
                Log.Error("XFrame", $"Release obj error, type not equal {obj.GetType().Name} {m_Type.Name}");
                return;
            }

            if (m_Full)
            {
                obj.OnDestroy(this);
            }
            else
            {
                obj.OnRelease(this);
                m_Objects[m_R] = obj;
                m_R = (m_R + 1) % m_Capacity;
            }
        }

        public IEnumerator<IPoolObject> GetEnumerator()
        {
            List<IPoolObject> objs = new List<IPoolObject>();
            for (int i = m_L; i != m_R; i = (i + 1) % m_Capacity)
            {
                objs.Add(m_Objects[i]);
            }
            return objs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
