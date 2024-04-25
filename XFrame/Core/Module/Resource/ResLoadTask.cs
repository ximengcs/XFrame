using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    public class ResLoadTask : XProTask
    {
        private IResHandler m_Handler;

        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
            m_Handler = handler;
        }

        protected override void InnerStart()
        {
            m_Handler.Start();
            base.InnerStart();
        }
    }

    public class ResLoadTask<T> : XProTask<T>
    {
        private IResHandler m_Handler;

        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
            m_Handler = handler;
        }

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
