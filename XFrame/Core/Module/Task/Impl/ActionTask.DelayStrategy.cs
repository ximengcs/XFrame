
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class DelayStrategy : ITaskStrategy<DelayHandler>
        {
            private DelayHandler m_Handler;
            private float m_Time;

            public void OnUse(DelayHandler handler)
            {
                m_Time = 0;
                m_Handler = handler;
            }

            public float OnHandle(ITask from)
            {
                m_Time += TimeModule.Inst.EscapeTime;

                if (m_Handler.Time <= m_Time)
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
