using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事辅助类
    /// </summary>
    public interface IPlotHelper
    {
        /// <summary>
        /// 剧情事件系统
        /// </summary>
        IEventSystem Event { get; }
    }
}
