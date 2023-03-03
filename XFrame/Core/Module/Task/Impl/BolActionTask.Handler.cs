using System;

namespace XFrame.Modules.Tasks
{
    public partial class BolActionTask
    {
        public class Handler : ITaskHandler
        {
            public Func<bool> Act;

            public Handler(Func<bool> act)
            {
                Act = act;
            }
        }
    }
}
