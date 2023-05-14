using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Pools
{
    internal class ObjectPool<T> : IPool<T> where T : IPoolObject
    {
        private Type m_Type;
        private IPoolHelper m_Helper;
        private XLinkList<IPoolObject> m_Objects;
        private XLoopQueue<XLinkNode<IPoolObject>> m_NodeCache;

        public Type ObjectType => m_Type;

        public ObjectPool()
        {
            m_Type = typeof(T);
            m_Objects = new XLinkList<IPoolObject>(false);
            m_NodeCache = new XLoopQueue<XLinkNode<IPoolObject>>(64);
        }

        public void SetHelper(IPoolHelper helper)
        {
            m_Helper = helper;
        }

        public T Require()
        {
            return (T)InnerRequire();
        }

        IPoolObject IPool.Require()
        {
            return InnerRequire();
        }

        public void Release(T obj)
        {
            InnerRelease(obj);
        }

        public void Release(IPoolObject obj)
        {
            InnerRelease(obj);
        }

        public void Spawn(int count)
        {
            for (int i = 0; i < count; i++)
            {
                IPoolObject obj = m_Helper.Factory(m_Type);
                obj.OnCreate();
                InnerRelease(obj);
            }
        }

        private IPoolObject InnerRequire()
        {
            IPoolObject obj;
            if (m_Objects.Empty)
            {
                obj = m_Helper.Factory(m_Type);
                obj.OnCreate();
            }
            else
            {
                XLinkNode<IPoolObject> node = m_Objects.RemoveFirstNode();
                obj = node.Value;
                node.OnDispose();
                if (m_NodeCache.Full)
                {
                    Log.Debug("XFrame", $"{m_Type.Name} pool node cache is full, the node will be gc");
                }
                else
                {
                    m_NodeCache.AddLast(node);
                }
            }

            obj.OnRequest();
            return obj;
        }

        private void InnerRelease(IPoolObject obj)
        {
            obj.OnRelease();
            if (m_NodeCache.Empty)
            {
                m_Objects.AddLast(obj);
            }
            else
            {
                XLinkNode<IPoolObject> node = m_NodeCache.RemoveFirst();
                node.Value = obj;
                m_Objects.AddLast(node);
            }
        }

        public void ClearObject()
        {
            foreach (XLinkNode<IPoolObject> obj in m_Objects)
                obj.Value.OnDelete();
            m_Objects.Clear();
        }
    }
}
