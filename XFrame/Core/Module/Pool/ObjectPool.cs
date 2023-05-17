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

        public ObjectPool(IPoolHelper helper)
        {
            m_Type = typeof(T); 
            m_Helper = helper;
            m_Objects = new XLinkList<IPoolObject>(false);
            m_NodeCache = new XLoopQueue<XLinkNode<IPoolObject>>(m_Helper.CacheCount);
        }

        public T Require(int poolKey)
        {
            return (T)InnerRequire(poolKey);
        }

        IPoolObject IPool.Require(int poolKey)
        {
            return InnerRequire(poolKey);
        }

        public void Release(T obj)
        {
            InnerRelease(obj);
        }

        public void Release(IPoolObject obj)
        {
            InnerRelease(obj);
        }

        public void Spawn(int poolKey, int count)
        {
            for (int i = 0; i < count; i++)
                InnerRelease(InnerCreate(poolKey));
        }

        private IPoolObject InnerCreate(int poolKey)
        {
            IPoolObject obj = m_Helper.Factory(m_Type, poolKey);
            obj.OnCreate();
            return obj;
        }

        private IPoolObject InnerRequire(int poolKey)
        {
            IPoolObject obj;
            if (m_Objects.Empty)
            {
                obj = InnerCreate(poolKey);
            }
            else
            {
                XLinkNode<IPoolObject> node = m_Objects.First;
                while (node != null)
                {
                    if (node.Value.PoolKey == poolKey)
                        break;
                }
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
