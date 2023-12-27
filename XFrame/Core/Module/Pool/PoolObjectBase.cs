
namespace XFrame.Modules.Pools
{
    public abstract class PoolObjectBase : IPoolObject
    {
        public string MarkName { get; set; }    

        public int PoolKey { get; protected set; }

        public IPool InPool { get; internal set; }

        /// <summary>
        /// 从对象池中创建时被调用
        /// </summary>
        protected internal virtual void OnCreateFromPool() { }

        /// <summary>
        /// 从对象池中请求时被调用
        /// </summary>
        protected internal virtual void OnRequestFromPool() { }

        /// <summary>
        /// 释放到对象池中时被调用
        /// </summary>
        protected internal virtual void OnReleaseFromPool() { }

        /// <summary>
        /// 从对象池中销毁时被调用
        /// </summary>
        protected internal virtual void OnDestroyFromPool() { }
    }
}
