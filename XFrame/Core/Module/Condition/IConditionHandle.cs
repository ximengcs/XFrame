using System;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public interface IConditionHandle : IDataProvider
    {
        int InstanceId { get; }

        /// <summary>
        /// 条件目标
        /// <para>
        /// <see cref="ConditionEvent.Target"/> 触发的目标会根据此值匹配句柄实例
        /// </para>
        /// <para>
        /// <see cref="IConditionCompare.Target"/> 具体的实现类会匹配到此值
        /// </para>
        /// </summary>
        int Target { get; }

        /// <summary>
        /// 条件需要达成的目标参数，如数量等
        /// </summary>
        UniversalParser Param { get; }

        /// <summary>
        /// 条件句柄所有条件组
        /// </summary>
        IConditionGroupHandle Group { get; }

        void Trigger(object oldValue, object newValue);

        void OnUpdate(Action<object, object> callback);

        void OnComplete(Action<IConditionHandle> callback);
    }
}
