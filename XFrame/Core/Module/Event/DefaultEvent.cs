using XFrame.Modules.Pools;

namespace XFrame.Modules.Event
{
    internal class DefaultEvent : XEvent
    {
        public static DefaultEvent Create(int eventId)
        {
            DefaultEvent evt = References.Require<DefaultEvent>();
            evt.Id = eventId;
            return evt;
        }
    }
}
