using System;

namespace XFrame.Modules.Tasks
{
    public partial class DelayTask
    {
        public class Handler : ITaskHandler
        {
            public Action Act;

            public float Time { get; private set; }

            public Handler(float time, Action handler)
            {
                Time = time;
                Act = handler;
            }
        }
    }
}
