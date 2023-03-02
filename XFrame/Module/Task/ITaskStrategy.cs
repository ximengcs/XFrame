using System;

namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务策略
    /// </summary>
    public interface ITaskStrategy
    {
        /// <summary>
        /// 标记开始使用此策略
        /// </summary>
        void OnUse();
    }

    public interface ITaskStrategy<T> : ITaskStrategy where T : ITaskHandler
    {
        /// <summary>
        /// 处理任务
        /// </summary>
        /// <param name="from">宿主任务</param>
        /// <param name="handler">要处理的目标</param>
        /// <returns>任务是否处理完成</returns>
        float Handle(ITask from, T handler);
    }
}
