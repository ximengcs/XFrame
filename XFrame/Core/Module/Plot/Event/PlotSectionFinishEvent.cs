
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事片段完成事件
    /// </summary>
    public class PlotSectionFinishEvent : XEvent
    {
        private static int s_EventId;

        /// <summary>
        /// 事件Id
        /// </summary>
        public static int EventId
        {
            get
            {
                if (s_EventId == default)
                    s_EventId = typeof(PlotSectionFinishEvent).GetHashCode();
                return s_EventId;
            }
        }

        /// <summary>
        /// 片段
        /// </summary>
        public ISection Seciton { get; private set; }

        private PlotSectionFinishEvent() { }

        /// <summary>
        /// 创建事件
        /// </summary>
        /// <param name="section">片段</param>
        /// <returns>事件实例</returns>
        public static PlotSectionFinishEvent Create(ISection section)
        {
            PlotSectionFinishEvent e = References.Require<PlotSectionFinishEvent>();
            e.Id = EventId;
            e.Seciton = section;
            return e;
        }
    }
}
