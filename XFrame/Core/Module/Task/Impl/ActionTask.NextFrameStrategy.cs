using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class NextFrameStrategy : ITaskStrategy<NextFrameHandler>
        {
            private NextFrameHandler m_Handler;

            public void OnUse(NextFrameHandler handler)
            {
                m_Handler = handler;
                m_Handler.Frame = TimeModule.Inst.Frame;
            }

            public float OnHandle(ITask from)
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

            public void OnFinish()
            {
                m_Handler = null;
            }
        }
    }
}
