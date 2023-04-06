using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    internal class DefaultPlotHelper : IPlotHelper
    {
        public IEventSystem OnNewStory { get; }

        public DefaultPlotHelper()
        {
            OnNewStory = EventModule.Inst.NewSys();
        }
    }
}
