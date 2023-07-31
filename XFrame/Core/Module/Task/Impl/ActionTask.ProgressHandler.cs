using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class ProgressHandler : ITaskHandler
        {
            public bool NextFrameExec;
            public long Frame;
            public Func<float> Act;

            public ProgressHandler(Func<float> act, bool nextFrame)
            {
                Act = act;
                NextFrameExec = nextFrame;
            }
        }
    }
}
