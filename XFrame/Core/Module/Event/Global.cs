
using XFrame.Core;

namespace XFrame.Modules.Event
{
    public static class Global
    {
        private static IEventSystem s_Inst;
        public static IEventSystem EventSystem
        {
            get
            {
                if (s_Inst == null)
                    s_Inst = ModuleUtility.Event.NewSys();
                return s_Inst;
            }
        }

        public static void Listen(int eventId, XEventHandler handler)
        {
            EventSystem.Listen(eventId, handler);
        }

        public static void Listen(int eventId, XEventHandler2 handler)
        {
            EventSystem.Listen(eventId, handler);
        }

        public static void Trigger(int eventId)
        {
            EventSystem.Trigger(eventId);
        }

        public static void Trigger(XEvent e)
        {
            EventSystem.Trigger(e);
        }

        public static void TriggerNow(int eventId)
        {
            EventSystem.TriggerNow(eventId);
        }

        public static void TriggerNow(XEvent e)
        {
            EventSystem.TriggerNow(e);
        }

        public static void Unlisten(int eventId, XEventHandler handler)
        {
            EventSystem.Unlisten(eventId, handler);
        }

        public static void Unlisten(int eventId, XEventHandler2 handler)
        {
            EventSystem.Unlisten(eventId, handler);
        }

        public static void Unlisten(int eventId)
        {
            EventSystem.Unlisten(eventId);
        }

        public static void Unlisten()
        {
            EventSystem.Unlisten();
        }
    }
}
