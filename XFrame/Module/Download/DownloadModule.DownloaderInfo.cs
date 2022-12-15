using System;

namespace XFrame.Modules.Download
{
    public partial class DownloadModule
    {
        private enum DownLoadType
        {
            Text,
            Bytes
        }

        private struct DownloadInfo
        {
            public int Times;
            public string Url;
            public DownLoadType Type;
            public Action<string> TextCallback;
            public Action<byte[]> BytesCallback;
            public Action ErrorCallback;
            public bool IsComplete;
        }
    }
}
