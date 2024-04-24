
namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 通用池化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommonPoolObject<T> : PoolObjectBase
    {
        /// <summary>
        /// 对象实例
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool Valid => Target != null;
    }
}
