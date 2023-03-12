
using XFrame.Modules.Tasks;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载辅助器
    /// </summary>
    public interface IDownloadHelper : ITaskHandler
    {
        /// <summary>
        /// 下载是否完成，成功或失败完成时为true
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// 下载结果
        /// </summary>
        DownloadResult Result { get; }

        string Url { get; set; }

        /// <summary>
        /// 请求下载
        /// </summary>
        /// <param name="url"></param>
        protected internal void Request();

        protected internal void OnInit();

        /// <summary>
        /// 更新状态
        /// </summary>
        protected internal void OnUpdate();

        /// <summary>
        /// 释放持有的资源
        /// </summary>
        protected internal void OnDispose();
    }
}
