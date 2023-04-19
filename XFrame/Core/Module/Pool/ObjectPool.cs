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
                obj = m_Helper.Factory(m_Type);
                obj.OnCreate();
                return true;
            }
            else
            {
                XLinkNode<IPoolObject> node = m_Objects.RemoveFirstNode();
                IPoolObject nodeOrigin = node;
                obj = node.Value;
                nodeOrigin.OnRelease();
                if (m_NodeCache.Full)
                {
                    Log.Debug("XFrame", $"{m_Type.Name} pool node cache is full, the node will be gc");
                }
                else
                {
                    m_NodeCache.AddLast(node);
                }

                obj.OnCreate();
                return false;
            }
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

        public void Dispose()
        {
            foreach (IPoolObject obj in m_Objects)
                obj.OnDelete();
            m_Objects.Clear();
        }
    }
}
