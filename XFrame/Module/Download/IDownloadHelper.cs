
namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载辅助器
    /// </summary>
    public interface IDownloadHelper
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
        /// 请求下载
        /// </summary>
        /// <param name="url"></param>
        void Request(string url);

        /// <summary>
        /// 更新状态
        /// </summary>
        void Update();

        /// <summary>
        /// 释放持有的资源
        /// </summary>
        void Dispose();
    }
}
