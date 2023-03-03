using System;

namespace XFrame.Modules.Tasks
{
    public partial class BolActionTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            public void OnUse()
            {

            }

            public float Handle(ITask from, Handler handler)
            {
                Func<bool> func = handler.Act;
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
        }
    }
}
