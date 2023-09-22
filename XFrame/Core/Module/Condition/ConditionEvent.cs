using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件事件
    /// <para>
    /// 此事件会影响所有条件组
    /// </para>
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
        /// 触发参数
        /// <para>
        /// 可为数量等, 定义为<see cref="IConditionCompare{}.OnEventTrigger"/>和
        /// <see cref="IConditionCompare{}.Check"/>的接受参数, 由<see cref="IConditionCompare"/>的具体实现类解析判断。
        /// </para>
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
            ConditionEvent evt = ModuleUtility.Pool.GetOrNew<ConditionEvent>().Require();
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
