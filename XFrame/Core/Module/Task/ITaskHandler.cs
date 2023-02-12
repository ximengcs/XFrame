using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 可处理的任务
    /// </summary>
    public interface ITaskHandler
    {
        /// <summary>
        /// 任务处理类型，需要匹配策略
        /// </summary>
        Type HandleType { get; }
    }
}
