
using System;
using XFrame.Core;

namespace XFrame.Tasks
{
    public partial class XTask
    {
        public static XProTask Delay(float time, bool nextFrameExec = true)
        {
            return new XProTask(new DelayHandler(time, nextFrameExec));
        }

        private class DelayHandler : IProTaskHandler
        {
            private float m_Target;
            private float m_Time;
            private float m_StartTime;
            private long m_StartFrame;
            private float m_Pro;

            public object Data => null;

            public bool IsDone
            {
                get
                {
                    if (XModule.Time.Frame < m_StartFrame)
                    {
                        m_Pro = XTaskHelper.MIN_PROGRESS;
                    }
                    else
                    {
                        m_Time += XModule.Time.EscapeTime;

                        if (m_Target <= m_Time)
                        {
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                        }
                        else
                        {
                            m_Pro = (m_Time - m_StartTime) / m_Time;
                        }
                    }
                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public float Pro => m_Pro;

            public DelayHandler(float target, bool nextFrameExec)
            {
                m_Target = target;
                m_Time = 0;
                m_StartFrame = nextFrameExec ? XModule.Time.Frame + 1 : 0;
                m_StartTime = XModule.Time.Time;
            }

            public void OnCancel()
            {

            }
        }
    }
}
