
namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            public void OnUse()
            {

            }

            public float Handle(ITask from, Handler handler)
            {
                handler.Act?.Invoke();
                return MAX_PRO;
            }
        }

    }
}
