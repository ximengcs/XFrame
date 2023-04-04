using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    public interface IPlotHelper
    {
        IEventSystem OnNewStory { get; }
    }
}
