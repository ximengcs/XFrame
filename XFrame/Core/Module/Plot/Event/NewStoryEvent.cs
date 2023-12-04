using System;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 新故事事件
    /// </summary>
    public class NewStoryEvent : XEvent
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
                    s_EventId = typeof(NewStoryEvent).GetHashCode();
                return s_EventId;
            }
        }

        /// <summary>
        /// 故事列表
        /// </summary>
        public IStory[] Stories { get; private set; }

        public static NewStoryEvent Create(IStory[] stories)
        {
            NewStoryEvent evt = References.Require<NewStoryEvent>();
            evt.Id = EventId;
            evt.Stories = stories;
            return evt;
        }

        public static NewStoryEvent Create(IStory story)
        {
            NewStoryEvent evt = References.Require<NewStoryEvent>();
            evt.Id = EventId;
            evt.Stories = new IStory[] { story };
            return evt;
        }
    }
}
