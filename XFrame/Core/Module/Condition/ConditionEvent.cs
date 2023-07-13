using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件事件
    /// </summary>
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

        /// <summary>
        /// 条件目标，即条件的类型
        /// </summary>
        public int Target { get; private set; }

        /// <summary>
        /// 触发参数，可为数量等，触发时会执行<see cref="IConditionCompare.Check(ConditionHandle, object)"/>生命周期方法，
        /// 其中<see cref="ConditionHandle.Target"/>句柄参数即和<see cref="Target"/>条件目标匹配的句柄，第二个参数传入此参数<see cref="Param"/>，
        /// 可由<see cref="IConditionCompare"/>的具体实现类解析判断。
        /// </summary>
        public object Param { get; private set; }

        /// <summary>
        /// 创建事件实例(从对象池中创建)
        /// </summary>
        /// <param name="target">条件目标</param>
        /// <param name="param">触发参数</param>
        /// <returns>事件实例</returns>
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
