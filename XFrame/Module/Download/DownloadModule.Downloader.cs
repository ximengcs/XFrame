using XFrame.Collections;

namespace XFrame.Modules
{
    public partial class DownloadModule
    {
        private class Downloader : XItem
        {
            private DownloadInfo m_Info;
            private IDownloadHelper m_Helper;

            public bool IsComplete => m_Info.IsComplete;

            public void Init(IDownloadHelper helper, DownloadInfo info)
            {
                m_Info = info;
                m_Helper = helper;
            }

            public void Start()
            {
                InnerRequest();
            }

            public void Update()
            {
                if (m_Info.IsComplete)
                    return;

                if (m_Helper == null)
                    return;

                m_Helper.Update();
                if (!m_Helper.IsDone)
                    return;

                DownloadResult result = m_Helper.Result;
                if (result.IsSuccess)
                {
                    switch (m_Info.Type)
                    {
                        case DownLoadType.Text:
                            m_Info.TextCallback?.Invoke(result.Text);
                            m_Info.TextCallback = null;
                            break;

                        case DownLoadType.Bytes:
                            m_Info.BytesCallback?.Invoke(result.Data);
                            m_Info.BytesCallback = null;
                            break;
                    }
                    m_Info.IsComplete = true;
                }
                else
                {
                    m_Info.Times--;
                    if (m_Info.Times <= 0)
                    {
                        Log.Debug("XFrame", $"Download {m_Info.Url} Error, stop download. reason: {result.ErrorReason}");
                        m_Info.ErrorCallback?.Invoke();
                        m_Info.IsComplete = true;
                    }
                    else
                    {
                        Log.Debug("XFrame", $"Download {m_Info.Url} Error, suplus times {m_Info.Times}");
                        InnerRequest();
                    }
                }
            }

            private void InnerRequest()
            {
                m_Helper?.Dispose();
                m_Helper.Request(m_Info.Url);
            }
        }
    }
}
