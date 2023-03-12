using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Download
{
    public class DownTask : TaskBase
    {
        public bool Success { get; private set; }

        public string Text { get; private set; }

        public byte[] Data { get; private set; }

        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        private class Strategy : ITaskStrategy<IDownloadHelper>
        {
            private const int TRY_TIMES = 8;
            private IDownloadHelper m_Handler;
            private int m_Times;
            private float m_Pro;
            private bool m_IsComplete;

            public void OnUse(IDownloadHelper handler)
            {
                m_Pro = 0;
                m_Times = TRY_TIMES;
                m_IsComplete = false;
                m_Handler = handler;
                InnerRequest();
            }

            public float OnHandle(ITask from)
            {
                if (m_IsComplete)
                    return m_Pro = MAX_PRO;

                m_Handler.OnUpdate();
                if (!m_Handler.IsDone)
                    return InnerNextPro();

                DownloadResult result = m_Handler.Result;
                DownTask task = (DownTask)from;
                if (result.IsSuccess)
                {
                    task.Data = result.Data;
                    task.Text = result.Text;
                    task.Success = true;
                    m_IsComplete = true;
                    m_Pro = MAX_PRO;
                }
                else
                {
                    m_Times--;
                    if (m_Times <= 0)
                    {
                        Log.Debug("XFrame", $"Download {m_Handler.Url} Error, stop download. reason: {result.ErrorReason}");
                        task.Success = false;
                        m_IsComplete = true;
                        m_Pro = MAX_PRO;
                    }
                    else
                    {
                        Log.Debug("XFrame", $"Download {m_Handler.Url} Error, reason: {result.ErrorReason}, suplus try times {m_Times}");
                        InnerRequest();
                        InnerNextPro();
                    }
                }

                return m_Pro;
            }

            public void OnFinish()
            {
                m_Handler = null;
            }

            private float InnerNextPro()
            {
                m_Pro = m_Pro + (1 - m_Pro) * 0.1f;
                return m_Pro;
            }

            private void InnerRequest()
            {
                m_Handler?.OnDispose();
                m_Handler.Request();
            }
        }
    }
}
