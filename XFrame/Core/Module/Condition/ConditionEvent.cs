using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    public class ConditionEvent : XEvent
    {
        private static int s_EventId;
        public static int EventId
        {
            get
            {
                if (s_EventId == default)
                    s_EventId = typeof(ConditionEvent).GetHashCode();
                return s_EventId;
            }
        }

        public int Target { get; private set; }
        public object Param { get; private set; }

        public static ConditionEvent Create(int target, object param)
        {
            ConditionEvent evt = PoolModule.Inst.GetOrNew<ConditionEvent>().Require();
            evt.Target = target;
            evt.Param = param;
            return evt;
        }

        protected internal override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            Id = EventId;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Param = default;
            Target = default;
        }
    }
}
