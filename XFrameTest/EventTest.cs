using XFrame.Core;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Event;

namespace XFrameTest
{
    class TestEvent : XEvent
    {
        public static int EventId => typeof(TestEvent).GetHashCode();

        public TestEvent() : base(EventId)
        {
        }
    }

    [TestClass]
    public class EventTest
    {
        static class GlobalEvent
        {
            private static IEventSystem m_System;

            public static void Init()
            {
                m_System = Entry.GetModule<IEventModule>().NewSys();
            }

            public static void Listen(int eventId, XEventHandler handler)
            {
                m_System.Listen(eventId, handler);
            }

            public static void Listen(int eventId, XEventHandler2 handler)
            {
                m_System.Listen(eventId, handler);
            }

            public static void Trigger(int eventId)
            {
                m_System.Trigger(eventId);
            }

            public static void Trigger(XEvent e)
            {
                m_System.Trigger(e);
            }

            public static void TriggerNow(int eventId)
            {
                m_System.TriggerNow(eventId);
            }

            public static void TriggerNow(XEvent e)
            {
                m_System.TriggerNow(e);
            }

            public static void Unlisten(int eventId, XEventHandler handler)
            {
                m_System.Unlisten(eventId, handler);
            }

            public static void Unlisten(int eventId, XEventHandler2 handler)
            {
                m_System.Unlisten(eventId, handler);
            }

            public static void Unlisten(int eventId)
            {
                m_System.Unlisten(eventId);
            }

            public static void Unlisten()
            {
                m_System.Unlisten();
            }
        }

        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                Log.ConsumeWaitQueue();
                Log.ToQueue = false;
                IEventSystem evtSys = Entry.GetModule<IEventModule>().NewSys();
                evtSys.Listen(TestEvent.EventId, (e) =>
                {
                    Log.Debug(e.Id);
                });
                evtSys.Trigger(new TestEvent());
                evtSys.TriggerNow(new TestEvent());
            });
        }

        [TestMethod]
        public void Test2()
        {
            EntryTest.Exec(() =>
            {
                GlobalEvent.Init();
                XEventHandler handler = (e) => Log.Debug("h1 " + e.Id);
                XEventHandler handler2 = (e) => Log.Debug("h2 " + e.Id);
                GlobalEvent.Listen(0, handler);
                GlobalEvent.Trigger(0);

                GlobalEvent.Listen(1, handler);
                GlobalEvent.Listen(1, handler2);
                GlobalEvent.TriggerNow(1);
                GlobalEvent.TriggerNow(1);

                GlobalEvent.Unlisten(1, handler2);
                GlobalEvent.TriggerNow(1);

                GlobalEvent.Unlisten();
                GlobalEvent.TriggerNow(0);
                GlobalEvent.TriggerNow(1);
            });
        }

        [TestMethod]
        public void Test3()
        {
            EntryTest.Exec(() =>
            {
                GlobalEvent.Init();
                XEventHandler handler = (e) => Log.Debug("h1 " + e.Id);
                XEventHandler2 handler2 = (e) =>
                {
                    Log.Debug("h2 " + e.Id);
                    return false;
                }; XEventHandler2 handler3 = (e) =>
                {
                    Log.Debug("h3 " + e.Id);
                    return true;
                };
                GlobalEvent.Listen(0, handler);
                GlobalEvent.Listen(0, handler2);
                GlobalEvent.Listen(0, handler3);
                GlobalEvent.TriggerNow(0);
                GlobalEvent.Trigger(0);
                GlobalEvent.Trigger(0);
                GlobalEvent.Trigger(0);
            });
        }
    }
}
