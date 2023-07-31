
using XFrame.Modules.Pools;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class RepeatStrategy : ITaskStrategy<RepeatHandler>
        {
            private CDTimer m_Timer;
            private RepeatHandler m_Handler;

            public void OnUse(RepeatHandler handler)
            {
                m_Handler = handler;
                m_Timer = CDTimer.Create();
                m_Timer.Record(m_Handler.TimeGap);
            }

            public float OnHandle(ITask from)
            {
                if (m_Timer.Check(true))
                    return m_Handler.Act() ? MAX_PRO : 0;
                else
                    return 0;
            }

            public void OnFinish()
            {
                m_Handler = null;
                References.Release(m_Timer);
            }
        }
    }
}
