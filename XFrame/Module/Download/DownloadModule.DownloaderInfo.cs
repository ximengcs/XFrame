
using System;

namespace XFrame.Modules
{
    public partial class DownloadModule
    {
        public enum DownLoadType
        {
            Text,
            Bytes
        }

        public struct DownloadInfo
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
