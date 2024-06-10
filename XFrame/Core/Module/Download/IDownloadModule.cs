
using XFrame.Core;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载器模块
    /// </summary>
    public interface IDownloadModule : IModule
    {
        /// <summary>
        /// 设置下载辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        void SetHelper<T>() where T : IDownloadHelper;

        /// <summary>
        /// 下载文件或数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="reserveUrls">url链接失败时备用url</param>
        DownTask Down(string url, params string[] reserveUrls);
    }
}
