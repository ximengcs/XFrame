﻿
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
                if (m_Handler.NextFrameExec)
                    m_Handler.Frame = TimeModule.Inst.Frame;
            }

            public float OnHandle(ITask from)
            {
                if (m_Handler.NextFrameExec)
                {
                    if (TimeModule.Inst.Frame <= m_Handler.Frame)
                        return 0;
                }

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