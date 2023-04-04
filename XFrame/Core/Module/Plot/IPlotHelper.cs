using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事辅助类
    /// </summary>
    public interface IPlotHelper
    {
        /// <summary>
        /// 新故事事件
        /// </summary>
        IEventSystem OnNewStory { get; }
    }
}
