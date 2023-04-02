using XFrame.Modules.Tasks;

namespace XFrame.Core
{
    /// <summary>
    /// 初始化处理器
    /// </summary>
    public interface IInitHandler : IEntryHandler
    {
        /// <summary>
        /// 入口处理
        /// </summary>
        void EnterHandle();

        /// <summary>
        /// 初始化之前处理
        /// </summary>
        /// <returns>此任务</returns>
        ITask BeforeHandle();

        /// <summary>
        /// 初始化之后处理
        /// </summary>
        /// <returns>此任务</returns>
        ITask AfterHandle();
    }
}
