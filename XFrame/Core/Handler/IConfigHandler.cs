
namespace XFrame.Core
{
    /// <summary>
    /// 配置处理类，可设置<see cref="XOrderAttribute"/>特性表明执行顺序
    /// </summary>
    public interface IConfigHandler
    {
        /// <summary>
        /// 此方法会在TypeModule执行后执行
        /// </summary>
        void OnHandle();
    }
}
