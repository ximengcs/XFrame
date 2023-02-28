using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载器模块
    /// </summary>
    public partial class DownloadModule : SingletonModule<DownloadModule>
    {
        #region Inner Fileds
        private const int TRY_TIMES = 8;
        private XLinkList<Downloader> m_Downloaders;
        private Type m_Helper;
        private IPool<Downloader> m_DownloaderPool;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Downloaders = new XLinkList<Downloader>();
            m_DownloaderPool = PoolModule.Inst.GetOrNew<Downloader>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            XLinkNode<Downloader> node = m_Downloaders.First;
            while (node != null)
            {
                Downloader downloader = node.Value;
                node = node.Next;
                if (downloader.IsComplete)
                    node.Delete();
                else
                    downloader.Update();
            }
        }
        #endregion

        #region Interface
        /// <summary>
        /// 设置下载辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        public void SetHelper<T>() where T : IDownloadHelper
        {
            m_Helper = typeof(T);
        }

        /// <summary>
        /// 下载一个文本
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="callback">成功回调</param>
        /// <param name="errorCallback">错误回调</param>
        public void DownText(string url, Action<string> callback, Action errorCallback = null)
        {
            InnerAddTask(url, DownLoadType.Text, callback, null, errorCallback);
        }

        /// <summary>
        /// 下载文件或数据
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="callback">成功回调</param>
        /// <param name="errorCallback">错误回调</param>
        public void DownData(string url, Action<byte[]> callback, Action errorCallback = null)
        {
            InnerAddTask(url, DownLoadType.Bytes, null, callback, errorCallback);
        }
        #endregion

        #region Inner Implement
        private void InnerAddTask(string url, DownLoadType type, Action<string> txtHandler, Action<byte[]> dataHandler, Action errorCallback)
        {
            DownloadInfo info = new DownloadInfo();
            info.Url = url;
            info.Type = type;
            info.Times = TRY_TIMES;
            info.TextCallback = txtHandler;
            info.BytesCallback = dataHandler;
            info.ErrorCallback = errorCallback;
            info.IsComplete = false;

            if (m_DownloaderPool.Require(out Downloader downloader))
                downloader.Init((IDownloadHelper)Activator.CreateInstance(m_Helper), info);
            downloader.Start();
            m_Downloaders.AddLast(downloader);
        }
        #endregion
    }
}
