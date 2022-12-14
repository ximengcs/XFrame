
namespace XFrame.Modules
{
    /// <summary>
    /// 资源加载任务处理器
    /// </summary>
    public interface IResHandler : ITaskHandler
    {
        /// <summary>
        /// 加载到的资源
        /// </summary>
        object Data { get; }

        /// <summary>
        /// 是否加载完成
        /// </summary>
        bool IsDone { get; }
    }
}
