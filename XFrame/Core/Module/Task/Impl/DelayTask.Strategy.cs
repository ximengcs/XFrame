using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class DelayTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            private float m_Time;

            public void OnUse()
            {
                m_Time = 0;
            }

            public float Handle(ITask from, Handler handler)
            {
                m_Time += TimeModule.Inst.EscapeTime;
                if (handler.Time <= m_Time)
                {
                    handler.Act?.Invoke();
                    return MAX_PRO;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
