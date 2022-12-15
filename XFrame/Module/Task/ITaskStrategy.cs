using System;

namespace XFrame.Modules
{
    /// <summary>
    /// 任务策略
    /// </summary>
    public interface ITaskStrategy
    {
        /// <summary>
        /// 可以处理的任务类型
        /// </summary>
        Type HandleType { get; }

        /// <summary>
        /// 处理任务
        /// </summary>
        /// <param name="from">宿主任务</param>
        /// <param name="target">要处理的目标</param>
        /// <returns>任务是否处理完成</returns>
        bool Handle(ITask from, ITaskHandler target);
    }
}
