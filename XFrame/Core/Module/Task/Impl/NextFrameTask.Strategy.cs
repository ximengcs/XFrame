using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class NextFrameTask
    {
        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            void ITaskStrategy<Handler>.OnUse(Handler handler)
            {
                m_Handler = handler;
            }

            float ITaskStrategy<Handler>.OnHandle(ITask from)
            {
                if (TimeModule.Inst.Frame > m_Handler.Frame)
                {
                    m_Handler.Act?.Invoke();
                    return MAX_PRO;
                }
                else
                {
                    return 0;
                }
            }

            void ITaskStrategy<Handler>.OnFinish()
            {
                m_Handler = null;
            }
        }
    }
}
