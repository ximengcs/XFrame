
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

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="text">文本</param>
        /// <param name="data">二进制数据</param>
        /// <param name="errorReason">错误原因</param>
        public DownloadResult(bool isSuccess, string text, byte[] data, string errorReason)
        {
            IsSuccess = isSuccess;
            Text = text;
            Data = data;
            ErrorReason = errorReason;
        }
    }
}
