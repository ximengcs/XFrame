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

        /// <summary>
        /// 下载任务url
        /// </summary>
        string Url { get; }

        /// <summary>
        /// 备用地址
        /// </summary>
        string[] ReserveUrl { get; }
    }
}
