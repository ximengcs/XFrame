using System;

namespace XFrame.Core.Binder
{
    /// <summary>
    /// 可更新值类型
    /// </summary>
    public interface IChangeableValue
    {
        /// <summary>
        /// 更新事件，当数值有更新时需要触发此事件
        /// </summary>
        event Action OnValueChange;
    }
}
