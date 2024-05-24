
namespace XFrame.Core.Threads
{
    /// <summary>
    /// 任务模块
    /// </summary>
    public interface ITaskModule : IModule, IUpdater
    {
        /// <summary>
        /// 注册可更新的任务
        /// </summary>
        /// <param name="task">任务</param>
        void Register(IUpdater task);

        /// <summary>
        /// 取消任务的注册
        /// </summary>
        /// <param name="task">任务</param>
        void UnRegister(IUpdater task);
    }
}
