using System;

namespace XFrame.Modules.Tasks
{
    public partial class ProActionTask
    {
        public class Strategy : ITaskStrategy<Handler>
        {
            public void OnUse()
            {

            }

            public float Handle(ITask from, Handler handler)
            {
                Func<float> func = handler.Act;
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
        }
    }
}
