
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
            }

            public float OnHandle(ITask from)
            {
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
