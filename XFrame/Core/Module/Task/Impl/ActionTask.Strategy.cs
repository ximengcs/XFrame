
using XFrame.Core;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
                if (m_Handler.NextFrameExec)
                    m_Handler.Frame = XModule.Time.Frame;
            }

            public float OnHandle(ITask from)
            {
                if (m_Handler.NextFrameExec)
                {
                    if (XModule.Time.Frame <= m_Handler.Frame)
                        return 0;
                }

                m_Handler.Act?.Invoke();
                return MAX_PRO;
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }

    }
}
