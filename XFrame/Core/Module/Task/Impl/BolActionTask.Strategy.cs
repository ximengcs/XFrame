using System;

namespace XFrame.Modules.Tasks
{
    public partial class BolActionTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
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
