using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    public class ResLoadTask : XProTask
    {
        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
        }
    }

    public class ResLoadTask<T> : XProTask<T>
    {
        public ResLoadTask(IResHandler handler, XTaskCancelToken cancelToken = null) : base(handler, cancelToken)
        {
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
