using XFrame.Tasks;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Download
{
    public partial class DownTask
    {
        private class Handler : IProTaskHandler
        {
            private const int TRY_TIMES = 8;
            private DownTask m_Task;
            private IDownloadHelper m_Helper;
            private int m_ReserveTryIndex;
            private int m_ReserveUrlCount;
            private int m_Times;
            private float m_Pro;
            private bool m_IsComplete;

            public object Data
            {
                get
                {
                    if (!string.IsNullOrEmpty(m_Task.Text))
                    {
                        return m_Task.Text;
                    }
                    else
                    {
                        return m_Task.Data;
                    }
                }
            }

            public bool IsDone
            {
                get
                {
                    InnerRefreshPro();
                    return m_IsComplete;
                }
            }

            public float Pro => m_Pro;

            public Handler(DownTask task, IDownloadHelper helper)
            {
                m_Pro = 0;
                m_ReserveTryIndex = 0;
                m_Times = TRY_TIMES;
                m_IsComplete = false;
                m_Task = task;
                m_Helper = helper;
                m_ReserveUrlCount = m_Helper.ReserveUrl != null ? m_Helper.ReserveUrl.Length : 0;
            }

            public void Start()
            {
                InnerRequest();
            }

            private float InnerRefreshPro()
            {
                if (m_IsComplete)
                    return XTaskHelper.MAX_PROGRESS;

                m_Helper.OnUpdate();
                if (!m_Helper.IsDone)
                    return InnerNextPro();

                DownloadResult result = m_Helper.Result;
                if (result.IsSuccess)
                {
                    Log.Debug(Log.XFrame, $"Download {m_Helper.Url} Successfully.");
                    m_Task.Data = result.Data;
                    m_Task.Text = result.Text;
                    m_Task.Success = true;
                    m_IsComplete = true;
                    m_Pro = XTaskHelper.MAX_PROGRESS;
                }
                else
                {
                    m_Times--;
                    if (m_Times <= 0)
                    {
                        Log.Debug(Log.XFrame, $"Download {m_Helper.Url} Error, stop download. reason: {result.ErrorReason}");

                        if (m_ReserveTryIndex < m_ReserveUrlCount)
                        {
                            m_Helper.Url = m_Helper.ReserveUrl[m_ReserveTryIndex++];
                            Log.Debug(Log.XFrame, $"Ready Download Reserve Url {m_Helper.Url}");
                            InnerRequest();
                        }
                        else
                        {
                            Log.Debug(Log.XFrame, $"Download Failure {m_Helper.Url}");
                            m_Task.Success = false;
                            m_IsComplete = true;
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                        }
                    }
                    else
                    {
                        Log.Debug(Log.XFrame, $"Download {m_Helper.Url} Error, reason: {result.ErrorReason}, suplus try times {m_Times}");
                        InnerRequest();
                        InnerNextPro();
                    }
                }

                return m_Pro;
            }

            public void OnCancel()
            {
                OnDispose();
            }

            public void OnDispose()
            {
                m_Helper.OnDispose();
                m_Helper = null;
            }

            private float InnerNextPro()
            {
                m_Pro = m_Pro + (1 - m_Pro) * 0.1f;
                return m_Pro;
            }

            private void InnerRequest()
            {
                m_Helper.OnDispose();
                m_Helper.Request();
            }
        }
    }
}
