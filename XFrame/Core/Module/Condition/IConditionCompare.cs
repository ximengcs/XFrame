﻿
namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件(单)检查器，比较器
    /// </summary>
    public interface IConditionCompare
    {
        /// <summary>
        /// 匹配条件目标，类型
        /// <para>
        /// 与<see cref="ConditionEvent.Target"/>以及<see cref="ConditionHandle.Target"/>相匹配
        /// </para>
        /// </summary>
        int Target { get; }

        /// <summary>
        /// 检查条件是否处于完成状态，在条件句柄初始化时会执行一次
        /// </summary>
        /// <param name="info">句柄</param>
        /// <returns>true为完成状态，反之亦然</returns>
        bool CheckFinish(ConditionHandle info);

        /// <summary>
        /// 检查条件是否完成，当<see cref="ConditionEvent"/>事件触发时，会将<see cref="ConditionEvent.Param"/>传入此方法方法检查
        /// 条件是否可以完成，一般需要此方法中检查到条件的目标数量等发生变化时，调用<see cref="ConditionHandle.Trigger"/>方法来触发
        /// 条件的目标数量更新
        /// <para>
        /// 注意若<see cref="ConditionHandle.Trigger"/>方法没有执行，则通过<see cref="ConditionHandle.OnUpdate"/>注册的回调永远不会执行，
        /// 即使在句柄完成之后，即完成之后只会执行<see cref="ConditionHandle.OnComplete"/>的回调
        /// </para>
        /// </summary>
        /// <param name="info">条件句柄</param>
        /// <param name="param">参数(事件参数<see cref="ConditionEvent.Param"/>)</param>
        /// <returns>返回true时，句柄会进入完成状态。反之亦然</returns>
        bool Check(ConditionHandle info, object param);
    }
}
