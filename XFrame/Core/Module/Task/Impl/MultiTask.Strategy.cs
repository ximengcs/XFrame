
namespace XFrame.Modules.Tasks
{
    public partial class MultiTask
    {
        private class Strategy : ITaskStrategy<Handler>
        {
            private Handler m_Handler;

            public void OnUse(Handler handler)
            {
                m_Handler = handler;
            }

            public float OnHandle(ITask from)
            {
                float all = m_Handler.Count;
                if (all <= 0)
                    return MAX_PRO;
                return m_Handler.Check() / all;
            }

            public void OnFinish()
            {
                m_Handler = null;
            }
        }
    }
}
