using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class Handler : ITaskHandler
        {
            public bool NextFrameExec;
            public long Frame;
            public Action Act;

            public Handler(Action act, bool nextFrameExec)
            {
                Act = act;
                NextFrameExec = nextFrameExec;
            }
        }
    }
}
