using XFrame.Modules.Pools;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件
    /// </summary>
    public abstract class XEvent : PoolObjectBase, IPoolObject
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

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Id = default;
        }
    }
}
