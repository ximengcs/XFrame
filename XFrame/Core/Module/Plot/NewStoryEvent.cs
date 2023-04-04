using System;
using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    public class NewStoryEvent : XEvent
    {
        public static int EventId => typeof(NewStoryEvent).GetHashCode();

        public Type TargetDirector { get; }
        public IStory[] Stories { get; }

        public NewStoryEvent(IStory[] stories, Type target = null) : base(EventId)
        {
            Stories = stories;
            TargetDirector = target;
        }
    }
}
