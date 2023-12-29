using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Pools
{
    internal class ObjectPool<T> : IPool<T> where T : IPoolObject
    {
        private Type m_Type;
        private PoolHelperBase m_Helper;
        private XLinkList<IXPoolObject> m_Objects;
        private XLoopQueue<XLinkNode<IXPoolObject>> m_NodeCache;
        private int m_UseCount;

        public Type ObjectType => m_Type;

        public int ObjectCount => m_Objects.Count;

        public int UseCount => m_UseCount;

        public IPoolHelper Helper => m_Helper;

        public IXEnumerable<IPoolObject> AllObjects
        {
            get
            {
                XLinkList<IPoolObject> list = new XLinkList<IPoolObject>();
                foreach (var objNode in m_Objects)
                    list.AddLast(objNode.Value);
                return list;
            }
        }

        public ObjectPool(PoolHelperBase helper)
        {
            m_UseCount = 0;
            m_Type = typeof(T);
            m_Helper = helper;
            m_Objects = new XLinkList<IXPoolObject>(false);
            m_NodeCache = new XLoopQueue<XLinkNode<IXPoolObject>>(m_Helper.CacheCount);
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

        private IXPoolObject InnerCreate(int poolKey, object userData)
        {
            IXPoolObject obj = m_Helper.Factory(m_Type, poolKey, userData);
            m_Helper.OnObjectCreate(obj);
            obj.OnCreateFromPool();
            return obj;
        }

        private IXPoolObject InnerRequire(int poolKey, object userData)
        {
            IXPoolObject obj;
            if (m_Objects.Empty)
            {
                obj = InnerCreate(poolKey, userData);
            }
            else
            {
                XLinkNode<IXPoolObject> node = m_Objects.First;
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
            obj.OnRequestFromPool();
            obj.InPool = this;
            return obj;
        }

        private bool InnerRelease(IPoolObject obj, bool check)
        {
            if (check && obj.InPool == null)
            {
                Log.Debug("XFrame", $"Pool Object {obj.GetType().Name} May Be Released, Release Failed");
                return false;
            }

            IXPoolObject xobj = obj as IXPoolObject;
            m_Helper.OnObjectRelease(xobj);
            xobj.OnReleaseFromPool();
            xobj.InPool = null;
            if (m_NodeCache.Empty)
            {
                m_Objects.AddLast(xobj);
            }
            else
            {
                XLinkNode<IXPoolObject> node = m_NodeCache.RemoveFirst();
                node.Value = xobj;
                m_Objects.AddLast(node);
            }
            return true;
        }

        public void ClearObject()
        {
            foreach (XLinkNode<IXPoolObject> obj in m_Objects)
            {
                m_Helper.OnObjectDestroy(obj);
                obj.Value.OnDestroyFromPool();
            }
            m_Objects.Clear();
            m_UseCount = 0;
        }
    }
}
