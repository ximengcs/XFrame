using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    public class ConditionGroupEvent : XEvent
    {
        private static int s_EventId;
        public static int EventId
        {
            get
            {
                if (s_EventId == 0)
                    s_EventId = typeof(ConditionGroupEvent).GetHashCode();
                return s_EventId;
            }
        }

        public IConditionGroupHandle Handle { get; private set; }
        public int Target { get; private set; }
        public object Param { get; private set; }

        public static ConditionGroupEvent Create(IConditionGroupHandle handle, int target, object param)
        {
            ConditionGroupEvent e = References.Require<ConditionGroupEvent>();
            e.Handle = handle;
            e.Target = target;
            e.Param = param;
            e.Id = EventId;
            return e;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Handle = null;
            Target = 0;
            Param = null;
        }
    }
}
