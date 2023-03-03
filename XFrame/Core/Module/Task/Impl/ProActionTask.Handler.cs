using System;

namespace XFrame.Modules.Tasks
{
    public partial class ProActionTask
    {
        public class Handler : ITaskHandler
        {
            public Func<float> Act;

            public Handler(Func<float> act)
            {
                Act = act;
            }
        }
    }
}
