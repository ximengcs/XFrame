using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class Handler : ITaskHandler
        {
            public Action Act;

            public Handler(Action act)
            {
                Act = act;
            }
        }
    }
}
