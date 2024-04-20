
using System;
using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    public class ResLoadTask_ : XProTask
    {
        private Action<object> m_OnDataComplete;

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public object Res => m_ProHandler.Data;

        public ResLoadTask_(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
        }

        public ITask OnCompleted(Action<object> handler)
        {
            if (m_OnComplete.IsComplete)
                handler(Res);
            else
                m_OnDataComplete += handler;
            return this;
        }

        protected override void InnerExecComplete()
        {
            if (m_OnDataComplete != null)
            {
                m_OnDataComplete(Res);
                m_OnDataComplete = null;
            }
            base.InnerExecComplete();
        }
    }

    public class ResLoadTask_<T> : XProTask
    {
        private Action<T> m_OnDataComplete;

        /// <summary>
        /// 加载到的资源
        /// </summary>
        public T Res => (T)m_ProHandler.Data;

        public ResLoadTask_(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {

        }

        public ITask OnCompleted(Action<T> handler)
        {
            if (m_OnComplete.IsComplete)
                handler(Res);
            else
                m_OnDataComplete += handler;
            return this;
        }

        protected override void InnerExecComplete()
        {
            if (m_OnDataComplete != null)
            {
                m_OnDataComplete(Res);
                m_OnDataComplete = null;
            }
            base.InnerExecComplete();
        }
    }
}
