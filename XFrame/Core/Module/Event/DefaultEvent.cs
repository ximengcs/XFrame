using XFrame.Modules.Pools;

namespace XFrame.Modules.Event
{
    internal class DefaultEvent : XEvent
    {
        public DefaultEvent() : base(default) { }

        public void SetId(int eventId)
        {
            Id = eventId;
        }
    }
}
