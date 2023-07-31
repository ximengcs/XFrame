using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class BoolHandler : ITaskHandler
        {
            public bool NextFrameExec;
            public long Frame;
            public Func<bool> Act;

            public BoolHandler(Func<bool> act, bool nextFrame)
            {
                Act = act;
                NextFrameExec = nextFrame;
            }
        }
    }
}
