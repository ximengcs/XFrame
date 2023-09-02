
using XFrame.Modules.Tasks;

namespace XFrame.Core.Caches
{
    public partial class CacheObjectTask
    {
        private class Strategy : ITaskStrategy<ICacheObjectFactory>
        {
            private ICacheObjectFactory m_Handler;

            public void OnUse(ICacheObjectFactory handler)
            {
                m_Handler = handler;
                m_Handler.OnFactory();
            }

            public float OnHandle(ITask from)
            {
                if (m_Handler.IsDone)
                {
                    CacheObjectTask task = from as CacheObjectTask;
                    task.CacheObject = m_Handler.Result;
                    return TaskBase.MAX_PRO;
                }
                else
                {
                    return 0;
                }
            }

            public void OnFinish()
            {
                m_Handler.OnFinish();
                m_Handler = null;
            }
        }
    }
}
