using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Pools
{
    internal class ObjectPool<T> : IPool<T> where T : IPoolObject
    {
        private Type m_Type;
        private IPoolHelper m_Helper;
        private IPoolModule m_Module;
        private XLinkList<IPoolObject> m_Objects;
        private XLoopQueue<XLinkNode<IPoolObject>> m_NodeCache;
        private int m_UseCount;

        public Type ObjectType => m_Type;

        public int ObjectCount => m_Objects.Count;

        public int UseCount => m_UseCount;

        public IPoolHelper Helper => m_Helper;

        public IPoolModule Module => m_Module;

        public IXEnumerable<IPoolObject> AllObjects => m_Objects;

        public ObjectPool(IPoolModule module, IPoolHelper helper)
        {
            m_UseCount = 0;
            m_Type = typeof(T);
            m_Helper = helper;
            m_Module = module;
            m_Objects = new XLinkList<IPoolObject>(false);
            m_NodeCache = new XLoopQueue<XLinkNode<IPoolObject>>(m_Helper.CacheCount);
        }

        public T Require(int poolKey, object userData = default)
        {
            T obj = (T)InnerRequire(poolKey, userData);
            m_UseCount++;
            return obj;
        }

        IPoolObject IPool.Require(int poolKey, object userData)
        {
            IPoolObject obj = InnerRequire(poolKey, userData);
            m_UseCount++;
            return obj;
        }

        public void Release(T obj)
        {
            if (InnerRelease(obj, true))
                m_UseCount--;
        }

        public void Release(IPoolObject obj)
        {
            if (InnerRelease(obj, true))
                m_UseCount--;
        }

        public void Spawn(int poolKey, int count, object userData = default, XLinkList<IPoolObject> toList = null)
        {
            for (int i = 0; i < count; i++)
            {
                IPoolObject obj = InnerCreate(poolKey, userData);
                InnerRelease(obj, false);
                if (toList != null)
                    toList.AddLast(obj);
            }
        }

        private IPoolObject InnerCreate(int poolKey, object userData)
        {
            IPoolObject obj = m_Helper.Factory(m_Type, poolKey, userData);
            obj.InPool = this;
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

        private bool InnerRelease(IPoolObject obj, bool check)
        {
            if (check && obj.InPool == null)
            {
                Log.Debug("XFrame", $"Pool Object {obj.GetType().Name} May Be Released, Release Failed");
                return false;
            }

            m_Helper.OnObjectRelease(obj);
            obj.OnRelease();
            obj.InPool = null;
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
            return true;
        }

        public void ClearObject()
        {
            foreach (XLinkNode<IPoolObject> obj in m_Objects)
            {
                m_Helper.OnObjectDestroy(obj);
                obj.Value.OnDelete();
            }
            m_Objects.Clear();
            m_UseCount = 0;
        }
    }
}
