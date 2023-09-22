using System;
using XFrame.Core;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class ProgressStrategy : ITaskStrategy<ProgressHandler>
        {
            private ProgressHandler m_Handler;

            public void OnUse(ProgressHandler handler)
            {
                m_Handler = handler;
                if (m_Handler.NextFrameExec)
                    m_Handler.Frame = ModuleUtility.Time.Frame;
            }

            public float OnHandle(ITask from)
            {
                if (m_Handler.NextFrameExec)
                {
                    if (ModuleUtility.Time.Frame <= m_Handler.Frame)
                        return 0;
                }

                Func<float> func = m_Handler.Act;
                if (func != null)
                {
                    float pro = func.Invoke();
                    if (pro > MAX_PRO)
                        pro = MAX_PRO;
                    return pro;
                }
                else
                {
                    return MAX_PRO;
                }
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }
    }
}
