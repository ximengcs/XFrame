using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事情节
    /// </summary>
    public interface ISection
    {
        IStory Story { get; }

        IDataProvider Data { get; }

        /// <summary>
        /// 情节是否结束
        /// </summary>
        bool IsDone { get; }
    }
}
