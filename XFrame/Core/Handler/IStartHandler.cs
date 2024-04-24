using XFrame.Tasks;

namespace XFrame.Core
{
    /// <summary>
    /// 启动处理器
    /// </summary>
    public interface IStartHandler : IEntryHandler
    {
        /// <summary>
        /// 启动之前处理
        /// </summary>
        /// <returns>此任务</returns>
        XTask BeforeHandle();

        /// <summary>
        /// 启动之后处理
        /// </summary>
        /// <returns></returns>
        XTask AfterHandle();
    }
}
