
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
        /// 下载任务url
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// 备用地址
        /// </summary>
        string[] ReserveUrl { get; set; }

        /// <summary>
        /// 请求下载
        /// </summary>
        /// <param name="url"></param>
        protected internal void Request();

        /// <summary>
        /// 初始化生命周期
        /// </summary>
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
