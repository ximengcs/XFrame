using XFrame.Tasks;

namespace XFrame.Modules.Caches
{
    /// <summary>
    /// 缓存对象工厂接口
    /// </summary>
    public interface ICacheObjectFactory : IProTaskHandler
    {
        /// <summary>
        /// 生产的对象
        /// </summary>
        ICacheObject Result { get; }

        /// <summary>
        /// 开始生产
        /// </summary>
        void OnFactory();
        /// <summary>
        /// 生产完成
        /// </summary>
        void OnFinish();
    }
}
