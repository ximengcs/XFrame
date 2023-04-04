using System;
using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 新故事事件
    /// </summary>
    public class NewStoryEvent : XEvent
    {
        /// <summary>
        /// 事件Id
        /// </summary>
        public static int EventId => typeof(NewStoryEvent).GetHashCode();

        /// <summary>
        /// 目标导演类
        /// </summary>
        public Type TargetDirector { get; }

        /// <summary>
        /// 故事列表
        /// </summary>
        public IStory[] Stories { get; }

        public NewStoryEvent(IStory[] stories, Type target = null) : base(EventId)
        {
            Stories = stories;
            TargetDirector = target;
        }
    }
}
