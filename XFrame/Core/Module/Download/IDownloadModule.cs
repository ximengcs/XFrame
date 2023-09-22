
using XFrame.Core;

namespace XFrame.Modules.Download
{
    public interface IDownloadModule : IModule
    {
        void SetHelper<T>() where T : IDownloadHelper;

        DownTask Down(string url, params string[] reserveUrls);
    }
}
