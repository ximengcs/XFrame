using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class RepeatHandler : ITaskHandler
        {
            public bool NextFrameExec;
            public long Frame;
            public float TimeGap;
            public Func<bool> Act;

            public RepeatHandler(float timeGap, Func<bool> act, bool nextFrame)
            {
                Act = act;
                TimeGap = timeGap;
                NextFrameExec = nextFrame;
            }
        }
    }
}
