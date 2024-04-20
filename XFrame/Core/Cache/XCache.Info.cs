using System;
using XFrame.Collections;
using XFrame.Modules.Tasks;

namespace XFrame.Core.Caches
{
    public static partial class XCache
    {
        public class ObjectCollection
        {
            private int m_CacheCount;
            private XLoopQueue<ICacheObject> Items;
            private ICacheObjectFactory m_Factory;

            public ICacheObjectFactory Factory => m_Factory;
            public Type TargetType { get; }

            public int Count => Items.Count;

            public bool HasItem => !Items.Empty;

            public ObjectCollection(Type target, ICacheObjectFactory factory, int cacheCount)
            {
                TargetType = target;
                m_CacheCount = cacheCount;
                m_Factory = factory;
                Items = new XLoopQueue<ICacheObject>(m_CacheCount);
                InnerCheckCache();
            }

            public ICacheObject Get()
            {
                if (HasItem)
                {
                    ICacheObject obj = Items.RemoveFirst();
                    InnerRequire();
                    return obj;
                }
                else
                {
                    return default;
                }
            }

            private void InnerCheckCache()
            {
                for (int i = 0; i < m_CacheCount; i++)
                    InnerRequire();
            }

            private void InnerRequire()
            {
                new CacheObjectTask(m_Factory).OnCompleted(InnerNewObject);
            }

            private void InnerNewObject(ICacheObject obj)
            {
                if (obj != null)
                    Items.AddLast(obj);
            }
        }
    }
}
