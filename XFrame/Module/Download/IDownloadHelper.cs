using System;

namespace XFrame.Modules
{
    public interface IDownloadHelper
    {
        bool IsDone { get; }
        DownloadResult Result { get; }
        void Request(string url);
        void Update();
        void Dispose();
    }
}
