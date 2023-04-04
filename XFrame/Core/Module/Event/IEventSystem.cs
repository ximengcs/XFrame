
namespace XFrame.Modules.Event
{
    public interface IEventSystem
    {
        void Trigger(XEvent e);
        void TriggerNow(XEvent e);
        void Listen(int eventId, XEventHandler handler);
        void Unlisten(int eventId, XEventHandler handler);
        void Unlisten(int eventId);
        void Unlisten();

        internal void OnUpdate();
    }
}
