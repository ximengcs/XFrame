
using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class DelayHandler : ITaskHandler
        {
            public bool NextFrameExec;
            public long Frame;
            public Action Act;

            public float Time { get; private set; }

            public DelayHandler(float time, Action handler, bool nextFrame)
            {
                Time = time;
                Act = handler;
                NextFrameExec = nextFrame;
            }
        }
    }
}
