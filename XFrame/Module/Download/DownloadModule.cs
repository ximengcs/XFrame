using System;
using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules
{
    public partial class DownloadModule : SingleModule<DownloadModule>
    {
        private const int TRY_TIMES = 8;
        private XCollection<Downloader> m_Downloaders;
        private Type m_Helper;
        private IPool m_DownloaderPool;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Downloaders = new XCollection<Downloader>();
            m_DownloaderPool = PoolModule.Inst.Get<IXItem>().Get<Downloader>();
        }

        public void Register<T>() where T : IDownloadHelper
        {
            m_Helper = typeof(T);
        }

        public void DownText(string url, Action<string> callback, Action errorCallback = null)
        {
            DownloadInfo info = new DownloadInfo();
            info.Url = url;
            info.Type = DownLoadType.Text;
            info.Times = TRY_TIMES;
            info.TextCallback = callback;
            info.ErrorCallback = errorCallback;
            info.IsComplete = false;

            if (m_DownloaderPool.Require(out Downloader downloader))
                downloader.Init((IDownloadHelper)Activator.CreateInstance(m_Helper), info);
            downloader.Start();
            m_Downloaders.Add(downloader);
        }

        public void DownData(string url, Action<byte[]> callback, Action errorCallback = null)
        {
            DownloadInfo info = new DownloadInfo();
            info.Url = url;
            info.Type = DownLoadType.Bytes;
            info.Times = TRY_TIMES;
            info.BytesCallback = callback;
            info.ErrorCallback = errorCallback;
            info.IsComplete = false;
            if (m_DownloaderPool.Require(out Downloader downloader))
                downloader.Init((IDownloadHelper)Activator.CreateInstance(m_Helper), info);
            downloader.Start();
            m_Downloaders.Add(downloader);
        }

        public override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            for (int i = m_Downloaders.Count - 1; i >= 0; i--)
            {
                Downloader helper = m_Downloaders.Get<Downloader>(i);
                if (helper.IsComplete)
                    m_Downloaders.Remove(helper);
                else
                    helper.Update();
            }
        }
    }
}
