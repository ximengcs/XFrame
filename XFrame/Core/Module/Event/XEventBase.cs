
namespace XFrame.Modules.Event
{
    public abstract class XEventBase : XEvent
    {
        public static int EventId { get; private set; }

        public XEventBase()
        {
            if (EventId == default)
                EventId = GetType().GetHashCode();
            Id = EventId;
        }
    }
}
