using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Conditions
{
    public interface IConditionGroupHandle : IDataProvider
    {
        string Name { get; }

        /// <summary>
        /// 条件是否完成
        /// </summary>
        bool Complete { get; }

        /// <summary>
        /// 条件配置
        /// </summary>
        ConditionSetting Setting { get; }

        /// <summary>
        /// 组内所有的条件
        /// </summary>
        List<IConditionHandle> AllInfo { get; }

        /// <summary>
        /// 组内还未达成的条件
        /// </summary>
        Dictionary<int, List<IConditionHandle>> NotInfo { get; }

        void OnComplete(Action<IConditionGroupHandle> callback);
    }
}
