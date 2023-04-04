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
        [TestMethod]
        public void Test1()
        {
            EntryTest.Exec(() =>
            {
                IEventSystem evtSys = EventModule.Inst.NewSys();
                evtSys.Listen(TestEvent.EventId, (e) =>
                {
                    Log.Debug(e.Id);
                });
                evtSys.Trigger(new TestEvent());
                evtSys.TriggerNow(new TestEvent());
            });
        }
    }
}
