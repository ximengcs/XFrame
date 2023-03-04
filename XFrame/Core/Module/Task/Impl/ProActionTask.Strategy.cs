using System;

namespace XFrame.Modules.Tasks
{
    public partial class ProActionTask
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
