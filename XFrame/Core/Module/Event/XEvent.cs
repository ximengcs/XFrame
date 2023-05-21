using XFrame.Modules.Pools;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件
    /// </summary>
    public abstract class XEvent : IPoolObject
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public int Id { get; protected set; }

        public XEvent(int id)
        {
            Id = id;
        }

        public XEvent() { }

        int IPoolObject.PoolKey => 0;

        void IPoolObject.OnCreate()
        {
            OnCreateFromPool();
        }

        void IPoolObject.OnRequest()
        {
            OnRequestFromPool();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
            Id = default;
        }

        protected internal virtual void OnCreateFromPool() { }
        protected internal virtual void OnRequestFromPool() { }
        protected internal virtual void OnDestroyFromPool() { }
        protected internal virtual void OnReleaseFromPool() { }
    }
}
