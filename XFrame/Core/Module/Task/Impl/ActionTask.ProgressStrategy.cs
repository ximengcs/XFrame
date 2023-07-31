using System;

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
            }

            public float OnHandle(ITask from)
            {
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
