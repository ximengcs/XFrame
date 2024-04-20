
using System;
using XFrame.Core;
using XFrame.Modules.Pools;
using XFrame.Modules.Times;

namespace XFrame.Tasks
{
    public partial class XTask
    {
        public static XProTask Condition(Func<bool> fun, bool nextFrameExec = true)
        {
            return new XProTask(new ConditionHandler(fun, nextFrameExec));
        }

        public static XProTask Condition(Func<Pair<bool, object>> fun, bool nextFrameExec = true)
        {
            return new XProTask(new ConditionWithDataHandler(fun, nextFrameExec));
        }

        public static XProTask Condition(Func<float> fun, bool nextFrameExec = true)
        {
            return new XProTask(new ConditionWithProgressHandler(fun, nextFrameExec));
        }

        public static XProTask Repeat(float timeGap, Func<bool> fun, bool nextFrameExec = true)
        {
            return new XProTask(new ConditionRepeatHandler(timeGap, fun, nextFrameExec));
        }

        private class ConditionHandler : IProTaskHandler
        {
            private Func<bool> m_Fun;
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
                        if (m_Fun())
                        {
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                        }
                    }
                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public float Pro => m_Pro;

            public ConditionHandler(Func<bool> fun, bool nextFrameExec = true)
            {
                m_Fun = fun;
                m_StartFrame = nextFrameExec ? XModule.Time.Frame + 1 : 0;
            }

            public void OnCancel()
            {

            }
        }

        private class ConditionRepeatHandler : IProTaskHandler
        {
            private CDTimer m_Timer;
            private Func<bool> m_Fun;
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
                        if (m_Timer.Check(true))
                            m_Pro = m_Fun() ? XTaskHelper.MAX_PROGRESS : XTaskHelper.MIN_PROGRESS;
                        else
                            m_Pro = XTaskHelper.MIN_PROGRESS;
                    }

                    bool done = m_Pro >= XTaskHelper.MAX_PROGRESS;
                    if (done)
                    {
                        References.Release(m_Timer);
                    }
                    return done;
                }
            }

            public float Pro => m_Pro;

            public ConditionRepeatHandler(float timeGap, Func<bool> fun, bool nextFrameExec = true)
            {
                m_Fun = fun;
                m_StartFrame = nextFrameExec ? XModule.Time.Frame + 1 : 0;
                m_Timer = CDTimer.Create();
                m_Timer.Record(timeGap);
            }

            public void OnCancel()
            {
                References.Release(m_Timer);
            }
        }

        private class ConditionWithDataHandler : IProTaskHandler
        {
            private Func<Pair<bool, object>> m_Fun;
            private long m_StartFrame;
            private float m_Pro;
            private Pair<bool, object> m_Result;

            public object Data => m_Result.Value;

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
                        m_Result = m_Fun();
                        if (m_Result.Key)
                        {
                            m_Pro = XTaskHelper.MAX_PROGRESS;
                        }
                    }
                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public float Pro => m_Pro;

            public ConditionWithDataHandler(Func<Pair<bool, object>> fun, bool nextFrameExec = true)
            {
                m_Fun = fun;
                m_StartFrame = nextFrameExec ? XModule.Time.Frame + 1 : 0;
            }

            public void OnCancel()
            {

            }
        }

        private class ConditionWithProgressHandler : IProTaskHandler
        {
            private Func<float> m_Fun;
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
                        m_Pro = m_Fun();
                    }
                    return m_Pro >= XTaskHelper.MAX_PROGRESS;
                }
            }

            public float Pro => m_Pro;

            public ConditionWithProgressHandler(Func<float> fun, bool nextFrameExec = true)
            {
                m_Fun = fun;
                m_StartFrame = nextFrameExec ? XModule.Time.Frame + 1 : 0;
            }

            public void OnCancel()
            {

            }
        }
    }
}
