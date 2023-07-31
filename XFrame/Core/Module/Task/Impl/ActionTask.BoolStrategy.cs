
using System;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class BoolStrategy : ITaskStrategy<BoolHandler>
        {
            private BoolHandler m_Handler;

            public void OnUse(BoolHandler handler)
            {
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

                Func<bool> func = m_Handler.Act;
                if (func != null)
                {
                    bool finish = func.Invoke();
                    return finish ? MAX_PRO : 0;
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
