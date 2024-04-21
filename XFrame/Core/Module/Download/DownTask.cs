using System;
using XFrame.Tasks;

namespace XFrame.Modules.Download
{
    public partial class DownTask : XProTask<DownTask>
    {
        private Handler m_Handler;

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

        public DownTask(IDownloadHelper handler, XTaskCancelToken cancelToken = null) : base(null, cancelToken)
        {
            m_Handler = new Handler(this, handler);
            m_ProHandler = m_Handler;
        }

        protected override void InnerStart()
        {
            base.InnerStart();
            m_Handler.Start();
        }

        public override DownTask GetResult()
        {
            return this;
        }

        public DownTask OnCompleted(Action<byte[]> callback)
        {
            m_BytesCallback += callback;
            return this;
        }

        public DownTask OnCompleted(Action<string> callback)
        {
            m_TextCallback += callback;
            return this;
        }

        protected override void InnerExecComplete()
        {
            if (Data != null)
                m_BytesCallback?.Invoke(Data);
            if (Text != null)
                m_TextCallback?.Invoke(Text);
            m_BytesCallback = null;
            m_TextCallback = null;
            m_Handler.OnDispose();
            m_Handler = null;
            base.InnerExecComplete();
        }
    }
}
