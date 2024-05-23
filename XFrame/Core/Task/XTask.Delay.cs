
namespace XFrame.Tasks
{
    public partial class XTask
    {
        /// <summary>
        /// 等待下一帧
        /// </summary>
        /// <returns>任务</returns>
        public static XProTask NextFrame()
        {
            return Delay(0);
        }

        /// <summary>
        /// 延迟任务
        /// </summary>
        /// <param name="time">时间，秒为单位</param>
        /// <param name="nextFrameExec">是否下一帧开始执行</param>
        /// <returns>任务</returns>
        public static XProTask Delay(float time, bool nextFrameExec = true)
        {
            return new XProTask(new DelayHandler(time, nextFrameExec));
        }

        private class DelayHandler : IProTaskHandler
        {
            private float m_Target;
            private double m_LastTime;
            private double m_Time;
            private long m_StartFrame;
            private double m_Pro;

            public object Data => null;

            public bool IsDone
            {
                get
                {
                    if (XTaskHelper.Time.Frame < m_StartFrame)
                    {
                        m_Pro = XTaskHelper.MIN_PROGRESS;
                    }
                    else
                    {
                        m_Time += XTaskHelper.Time.Time - m_LastTime;
                        m_LastTime = XTaskHelper.Time.Time;

                        if (m_Target <= m_Time)
                        {
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                        }
                        else
                        {
                            m_Pro = m_Time / m_Target;
                        }
                    }

                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public double Pro => m_Pro;

            public DelayHandler(float target, bool nextFrameExec)
            {
                m_Target = target;
                m_Time = 0;
                m_LastTime = XTaskHelper.Time.Time;
                m_StartFrame = nextFrameExec ? XTaskHelper.Time.Frame + 1 : 0;
            }

            public void OnCancel()
            {

            }
        }
    }
}
