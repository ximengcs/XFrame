
using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class DelayHandler : ITaskHandler
        {
            public Action Act;

            public float Time { get; private set; }

            public DelayHandler(float time, Action handler)
            {
                Time = time;
                Act = handler;
            }
        }
    }
}
