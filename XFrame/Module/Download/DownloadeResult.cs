
namespace XFrame.Modules
{
    public struct DownloadResult
    {
        public bool IsSuccess { get; }
        public string Text { get; }
        public byte[] Data { get; }
        public string ErrorReason { get; }

        public DownloadResult(bool isSuccess, string text, byte[] data, string errorReason)
        {
            IsSuccess = isSuccess;
            Text = text;
            Data = data;
            ErrorReason = errorReason;
        }
    }
}
