using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    internal class DefaultPlotHelper : IPlotHelper
    {
        public IEventSystem Event { get; }

        public DefaultPlotHelper()
        {
            Event = EventModule.Inst.NewSys();
        }
    }
}
