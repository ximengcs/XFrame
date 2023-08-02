
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件事件
    /// <para>
    /// 此事件仅会影响目标句柄<see cref="Handle"/>的指定条件，
    /// 但当触发的条件是全局条件时，则会触发全局<see cref="IConditionCompare{}.OnEventTrigger"/>
    /// </para>
    /// </summary>
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

        /// <summary>
        /// 目标条件句柄
        /// </summary>
        public IConditionHandle Handle { get; private set; }

        /// <summary>
        /// 触发参数
        /// </summary>
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
