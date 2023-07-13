using XFrame.Modules.Tasks;
using XFrame.Modules.Diagnotics;
using System;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载任务
    /// </summary>
    public class DownTask : TaskBase
    {
        private Action<byte[]> m_BytesCallback;
        private Action<string> m_TextCallback;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// 文本数据
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 字节数据
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public DownTask OnComplete(Action<byte[]> callback)
        {
            m_BytesCallback += callback;
            return this;
        }

        public DownTask OnComplete(Action<string> callback)
        {
            m_TextCallback += callback;
            return this;
        }

        protected override void InnerComplete()
        {
            base.InnerComplete();
            if (Data != null)
                m_BytesCallback?.Invoke(Data);
            if (Text != null)
                m_TextCallback?.Invoke(Text);
            m_BytesCallback = null;
            m_TextCallback = null;
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
                m_Handler?.OnDispose();
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
