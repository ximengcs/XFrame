using XFrame.Modules.Tasks;

namespace XFrame.Modules.Resource
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

        /// <summary>
        /// 加载进度
        /// </summary>
        float Pro { get; }

        /// <summary>
        /// 开始请求资源
        /// </summary>
        void Start();

        /// <summary>
        /// 释放请求
        /// </summary>
        void Dispose();
    }
}
