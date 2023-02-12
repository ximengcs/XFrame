
namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池对象
    /// </summary>
    public interface IPoolObject
    {
        /// <summary>
        /// 从对象池中创建时被调用
        /// </summary>
        protected internal void OnCreate();

        /// <summary>
        /// 释放到对象池中时被调用
        /// </summary>
        protected internal void OnRelease();

        /// <summary>
        /// 释放到对象池失败时被调用或永久销毁时调用
        /// </summary>
        protected internal void OnDestroyForever();
    }
}
