
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    public class SpecificConditionEvent : XEvent
    {
        private static int s_EventId;
        public static int EventId
        {
            get
            {
                if (s_EventId == 0)
                    s_EventId = typeof(SpecificConditionEvent).GetHashCode();
                return s_EventId;
            }
        }

        public IConditionHandle Handle { get; private set; }
        public object Param { get; private set; }

        public static SpecificConditionEvent Create(IConditionHandle handle, object param)
        {
            SpecificConditionEvent e = References.Require<SpecificConditionEvent>();
            e.Handle = handle;
            e.Param = param;
            e.Id = EventId;
            return e;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Handle = null;
            Param = null;
        }
    }
}
