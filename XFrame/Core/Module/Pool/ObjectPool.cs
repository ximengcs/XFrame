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

        public IPoolHelper Helper => m_Helper;

        public ObjectPool(IPoolHelper helper)
        {
            m_Type = typeof(T);
            m_Helper = helper;
            m_Objects = new XLinkList<IPoolObject>(false);
            m_NodeCache = new XLoopQueue<XLinkNode<IPoolObject>>(m_Helper.CacheCount);
        }

        public T Require(int poolKey, object userData = default)
        {
            return (T)InnerRequire(poolKey, userData);
        }

        IPoolObject IPool.Require(int poolKey, object userData)
        {
            return InnerRequire(poolKey, userData);
        }

        public void Release(T obj)
        {
            InnerRelease(obj);
        }

        public void Release(IPoolObject obj)
        {
            InnerRelease(obj);
        }

        public XLinkList<IPoolObject> Spawn(int poolKey, int count, object userData = default)
        {
            XLinkList<IPoolObject> result = References.Require<XLinkList<IPoolObject>>();
            for (int i = 0; i < count; i++)
            {
                IPoolObject obj = InnerCreate(poolKey, userData);
                InnerRelease(obj);
                result.AddLast(obj);
            }
            return result;
        }

        private IPoolObject InnerCreate(int poolKey, object userData)
        {
            IPoolObject obj = m_Helper.Factory(m_Type, poolKey, userData);
            m_Helper.OnObjectCreate(obj);
            obj.OnCreate();
            return obj;
        }

        private IPoolObject InnerRequire(int poolKey, object userData)
        {
            IPoolObject obj;
            if (m_Objects.Empty)
            {
                obj = InnerCreate(poolKey, userData);
            }
            else
            {
                XLinkNode<IPoolObject> node = m_Objects.First;
                while (node != null)
                {
                    if (node.Value.PoolKey == poolKey)
                        break;
                    node = node.Next;
                }
                if (node != null)
                {
                    obj = node.Value;
                    node.Delete();
                    if (m_NodeCache.Full)
                    {
                        Log.Debug("XFrame", $"{m_Type.Name} pool node cache is full, the node will be gc");
                    }
                    else
                    {
                        m_NodeCache.AddLast(node);
                    }
                }
                else
                {
                    obj = InnerCreate(poolKey, userData);
                }
            }

            m_Helper.OnObjectRequest(obj);
            obj.OnRequest();
            return obj;
        }

        private void InnerRelease(IPoolObject obj)
        {
            m_Helper.OnObjectRelease(obj);
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
            {
                m_Helper.OnObjectDestroy(obj);
                obj.Value.OnDelete();
            }
            m_Objects.Clear();
        }
    }
}
