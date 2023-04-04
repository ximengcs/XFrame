
namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事情节状态
    /// </summary>
    public enum SectionState
    {
        /// <summary>
        /// 等待初始化
        /// </summary>
        WaitInit,

        /// <summary>
        /// 等待开始
        /// </summary>
        WaitStart,

        /// <summary>
        /// 播放中
        /// </summary>
        Running,

        /// <summary>
        /// 已完成
        /// </summary>
        Finish
    }
}
