using System;
using XFrame.Tasks;

namespace XFrame.Modules.Download
{
    /// <summary>
    /// 下载任务
    /// </summary>
    public partial class DownTask : XProTask<DownTask>
    {
        private Handler m_Handler;

        private Action<byte[]> m_BytesCallback;
        private Action<string> m_TextCallback;

        public bool Disposed => m_OnComplete.IsComplete;

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
        /// 构造器
        /// </summary>
        /// <param name="handler">下载辅助器</param>
        /// <param name="cancelToken">取消Token</param>
        public DownTask(IDownloadHelper handler, XTaskCancelToken cancelToken = null) : base(null, cancelToken)
        {
            m_Handler = new Handler(this, handler);
            m_ProHandler = m_Handler;
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        protected override void InnerStart()
        {
            base.InnerStart();
            m_Handler.Start();
        }

        /// <summary>
        /// 获取结果
        /// </summary>
        /// <returns>下载任务</returns>
        public override DownTask GetResult()
        {
            return this;
        }

        /// <summary>
        /// 设置完成回调
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns>下载任务</returns>
        public DownTask OnCompleted(Action<byte[]> callback)
        {
            m_BytesCallback += callback;
            return this;
        }

        /// <summary>
        /// 设置完成回调
        /// </summary>
        /// <param name="callback">回调函数</param>
        /// <returns>下载任务</returns>
        public DownTask OnCompleted(Action<string> callback)
        {
            m_TextCallback += callback;
            return this;
        }

        /// <summary>
        /// 任务完成
        /// </summary>
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
