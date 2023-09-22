
namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池对象
    /// </summary>
    public interface IPoolObject
    {
        int PoolKey { get; }

        string MarkName { get; set; }

        IPool InPool { get; protected internal set; }

        /// <summary>
        /// 从对象池中创建时被调用
        /// </summary>
        protected internal void OnCreate();

        /// <summary>
        /// 从对象池中请求时被调用
        /// </summary>
        protected internal void OnRequest();

        /// <summary>
        /// 释放到对象池中时被调用
        /// </summary>
        protected internal void OnRelease();

        /// <summary>
        /// 从对象池中销毁时被调用
        /// </summary>
        protected internal void OnDelete();
    }
}
