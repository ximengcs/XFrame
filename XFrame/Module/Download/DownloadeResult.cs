
namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载结果
    /// </summary>
    public struct DownloadResult
    {
        /// <summary>
        /// 是否下载成功
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// 下载的文件是文本时有值
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// 下载的文件不是文本时为字节数据
        /// </summary>
        public byte[] Data { get; }

        /// <summary>
        /// 下载失败时的失败原因，成功时为空
        /// </summary>
        public string ErrorReason { get; }

        internal DownloadResult(bool isSuccess, string text, byte[] data, string errorReason)
        {
            IsSuccess = isSuccess;
            Text = text;
            Data = data;
            ErrorReason = errorReason;
        }
    }
}
