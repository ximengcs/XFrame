
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Plots
{
    public class PlotSectionFinishEvent : XEvent
    {
        private static int s_EventId;

        public static int EventId
        {
            get
            {
                if (s_EventId == default)
                    s_EventId = typeof(PlotSectionFinishEvent).GetHashCode();
                return s_EventId;
            }
        }

        public ISection Seciton { get; private set; }

        private PlotSectionFinishEvent() { }

        public static PlotSectionFinishEvent Create(ISection section)
        {
            PlotSectionFinishEvent e = References.Require<PlotSectionFinishEvent>();
            e.Id = EventId;
            e.Seciton = section;
            return e;
        }
    }
}
