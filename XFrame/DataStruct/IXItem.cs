using XFrame.Modules;

namespace XFrame.Collections
{
    /// <summary>
    /// XCollection集合元素
    /// </summary>
    public interface IXItem : IPoolObject
    {
        /// <summary>
        /// 元素Id
        /// </summary>
        int Id { get; }
    }
}
