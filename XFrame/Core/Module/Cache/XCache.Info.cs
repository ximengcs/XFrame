using System;
using XFrame.Collections;

namespace XFrame.Modules.Caches
{
    public partial class XCache
    {
        /// <summary>
        /// 缓存对象集合
        /// </summary>
        public class ObjectCollection
        {
            private int m_CacheCount;
            private XLoopQueue<ICacheObject> Items;
            private ICacheObjectFactory m_Factory;

            /// <summary>
            /// 生成工厂
            /// </summary>
            public ICacheObjectFactory Factory => m_Factory;

            /// <summary>
            /// 目标类型
            /// </summary>
            public Type TargetType { get; }

            /// <summary>
            /// 缓存数量
            /// </summary>
            public int Count => Items.Count;

            /// <summary>
            /// 是否为空
            /// </summary>
            public bool HasItem => !Items.Empty;

            /// <summary>
            /// 构造器
            /// </summary>
            /// <param name="target">对象类型</param>
            /// <param name="factory">对象工厂</param>
            /// <param name="cacheCount">缓存数量</param>
            public ObjectCollection(Type target, ICacheObjectFactory factory, int cacheCount)
            {
                TargetType = target;
                m_CacheCount = cacheCount;
                m_Factory = factory;
                Items = new XLoopQueue<ICacheObject>(m_CacheCount);
                InnerCheckCache();
            }

            /// <summary>
            /// 获取一个缓存对象
            /// </summary>
            /// <returns>缓存对象</returns>
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
                new CacheObjectTask(m_Factory)
                    .OnCompleted(InnerNewObject)
                    .Coroutine();
            }

            private void InnerNewObject(ICacheObject obj)
            {
                if (obj != null)
                    Items.AddLast(obj);
            }
        }
    }
}
