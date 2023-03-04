
namespace XFrame.Modules.Tasks
{
    /// <summary>
    /// 任务策略
    /// </summary>
    public interface ITaskStrategy
    {
        
    }

    public interface ITaskStrategy<T> : ITaskStrategy where T : ITaskHandler
    {
        /// <summary>
        /// 标记开始使用此策略
        /// <param name="handler">要处理的目标</param>
        /// </summary>
        void OnUse(T handler);

        /// <summary>
        /// 处理任务
        /// </summary>
        /// <param name="from">宿主任务</param>
        /// <returns>任务是否处理完成</returns>
        float OnHandle(ITask from);

        /// <summary>
        /// 标记此任务完成
        /// </summary>
        void OnFinish();
    }
}
