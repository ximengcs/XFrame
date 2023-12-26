
namespace XFrame.Modules.Download
{
    public abstract class DownloadHelperBase : IDownloadHelper
    {
        public bool IsDone { get; protected set; }

        public DownloadResult Result { get; protected set; }

        public string Url { get; internal set; }

        public string[] ReserveUrl { get; internal set; }

        /// <summary>
        /// 请求下载
        /// </summary>
        /// <param name="url"></param>
        protected internal abstract void Request();

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal abstract void OnInit();

        /// <summary>
        /// 更新状态
        /// </summary>
        protected internal abstract void OnUpdate();

        /// <summary>
        /// 释放持有的资源
        /// </summary>
        protected internal abstract void OnDispose();
    }
}
