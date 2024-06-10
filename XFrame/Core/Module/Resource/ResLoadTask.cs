using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源加载任务
    /// </summary>
    public class ResLoadTask : XProTask
    {
        private IResHandler m_Handler;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="handler">资源处理器</param>
        /// <param name="cancelToken">取消绑定器</param>
        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
            m_Handler = handler;
        }

        /// <inheritdoc/>
        protected override void InnerStart()
        {
            m_Handler.Start();
            base.InnerStart();
        }
    }

    /// <summary>
    /// 资源加载任务
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    public class ResLoadTask<T> : XProTask<T>
    {
        private IResHandler m_Handler;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="handler">资源处理器</param>
        /// <param name="cancelToken">取消绑定器</param>
        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
            m_Handler = handler;
        }

        /// <inheritdoc/>
        protected override void InnerStart()
        {
            m_Handler.Start();
            base.InnerStart();
        }

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public override T GetResult()
        {
            return (T)m_ProHandler.Data;
        }
    }
}
