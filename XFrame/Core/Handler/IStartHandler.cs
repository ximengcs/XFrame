using XFrame.Modules.Tasks;

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
        ITask BeforeHandle();

        /// <summary>
        /// 启动之后处理
        /// </summary>
        /// <returns></returns>
        ITask AfterHandle();
    }
}
