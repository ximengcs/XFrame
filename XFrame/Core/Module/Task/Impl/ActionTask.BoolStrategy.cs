
using System;

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
            }

            public float OnHandle(ITask from)
            {
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
